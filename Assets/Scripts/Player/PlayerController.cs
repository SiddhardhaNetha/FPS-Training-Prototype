using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = -20f;

    [Header("Look")]
    public float mouseSensitivity = 0.1f;
    public float mobileLookSensitivity = 0.15f;
    public Transform playerCamera;

    [Header("Camera Recoil")]
    public float recoilAmount = 2f;
    public float recoilRecoverySpeed = 10f;

    [Header("Mobile")]
    public MobileJoystick mobileJoystick;
    public MobileLookArea mobileLookArea;
    public MobileFireButton mobileFireButton;

    private CharacterController controller;
    private float verticalVelocity;
    private float cameraPitch;

    private float recoilOffset;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        mouseSensitivity =
            PlayerPrefs.GetFloat(
                "Sensitivity",
                mouseSensitivity
            );

        mobileLookSensitivity =
            PlayerPrefs.GetFloat(
                "Sensitivity",
                mobileLookSensitivity
            );

        UnlockCursor();
    }

    private void Update()
    {
        HandleCursor();
        Move();
        Look();
        HandleRecoil();
    }

    // =========================================================
    // CURSOR
    // =========================================================

    private void HandleCursor()
    {
        // ESC = unlock cursor
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            UnlockCursor();
            return;
        }

        // Click the game world to lock cursor
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame &&
            !IsPointerOverUI())
        {
            LockCursor();
        }
    }

    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;

        return EventSystem.current.IsPointerOverGameObject();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // =========================================================
    // MOVEMENT
    // =========================================================

    private void Move()
    {
        Vector2 input = Vector2.zero;

        // -----------------------------------------------------
        // MOBILE JOYSTICK
        // -----------------------------------------------------

        if (mobileJoystick != null)
        {
            input = new Vector2(
                mobileJoystick.Horizontal,
                mobileJoystick.Vertical
            );
        }

        // -----------------------------------------------------
        // PC KEYBOARD
        // -----------------------------------------------------

        if (Keyboard.current != null)
        {
            Vector2 keyboardInput = new Vector2(
                (Keyboard.current.dKey.isPressed ? 1 : 0) -
                (Keyboard.current.aKey.isPressed ? 1 : 0),

                (Keyboard.current.wKey.isPressed ? 1 : 0) -
                (Keyboard.current.sKey.isPressed ? 1 : 0)
            );

            if (keyboardInput.sqrMagnitude > 0f)
            {
                input = keyboardInput;
            }
        }

        input = Vector2.ClampMagnitude(input, 1f);

        Vector3 move =
            transform.right * input.x +
            transform.forward * input.y;

        controller.Move(
            move * moveSpeed * Time.deltaTime
        );

        // -----------------------------------------------------
        // GROUND / GRAVITY
        // -----------------------------------------------------

        if (controller.isGrounded &&
            verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity +=
            gravity * Time.deltaTime;

        controller.Move(
            Vector3.up *
            verticalVelocity *
            Time.deltaTime
        );
    }

    // =========================================================
    // LOOK
    // =========================================================


    private void Look()
    {
        // =====================================================
        // MOBILE DEVICE
        // =====================================================

        if (Application.isMobilePlatform)
        {
            // ---------------------------------------------
            // NORMAL LOOK AREA
            // ---------------------------------------------

            if (mobileLookArea != null)
            {
                Vector2 mobileDelta =
                    mobileLookArea.GetLookDelta();

                if (mobileDelta.sqrMagnitude > 0.001f)
                {
                    MobileLook(
                        mobileDelta * mobileLookSensitivity
                    );
                }
            }

            // ---------------------------------------------
            // FIRE BUTTON LOOK
            // Same finger can FIRE + AIM
            // ---------------------------------------------

            if (mobileFireButton != null)
            {
                Vector2 fireLookDelta =
                    mobileFireButton.GetLookDelta();

                if (fireLookDelta.sqrMagnitude > 0.001f)
                {
                    MobileLook(
                        fireLookDelta * mobileLookSensitivity
                    );
                }
            }

            return;
        }

        // =====================================================
        // PC / UNITY EDITOR MOUSE LOOK
        // =====================================================

        if (Mouse.current == null)
            return;

        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        Vector2 mouseDelta =
            Mouse.current.delta.ReadValue();

        float mouseX =
            mouseDelta.x * mouseSensitivity;

        float mouseY =
            mouseDelta.y * mouseSensitivity;

        // Rotate player horizontally
        transform.Rotate(
            0f,
            mouseX,
            0f
        );

        // Rotate camera vertically
        cameraPitch -= mouseY;

        cameraPitch = Mathf.Clamp(
            cameraPitch,
            -90f,
            90f
        );

        ApplyCameraRotation();
    }

    // =========================================================
    // MOBILE LOOK
    // =========================================================

    public void MobileLook(Vector2 delta)
    {
        float mouseX = delta.x;
        float mouseY = delta.y;

        // Horizontal rotation
        transform.Rotate(
            0f,
            mouseX,
            0f
        );

        // Vertical rotation
        cameraPitch -= mouseY;

        cameraPitch = Mathf.Clamp(
            cameraPitch,
            -90f,
            90f
        );

        ApplyCameraRotation();
    }

    // =========================================================
    // CAMERA ROTATION
    // =========================================================

    private void ApplyCameraRotation()
    {
        if (playerCamera == null)
            return;

        playerCamera.localRotation =
            Quaternion.Euler(
                cameraPitch - recoilOffset,
                0f,
                0f
            );
    }

    // =========================================================
    // RECOIL
    // =========================================================

    private void HandleRecoil()
    {
        // Smoothly return recoil to zero
        recoilOffset = Mathf.Lerp(
            recoilOffset,
            0f,
            recoilRecoverySpeed * Time.deltaTime
        );

        ApplyCameraRotation();
    }

    // =========================================================
    // ADD RECOIL
    // =========================================================

    public void AddRecoil()
    {
        recoilOffset += recoilAmount;

        // Prevent excessive recoil
        recoilOffset = Mathf.Clamp(
            recoilOffset,
            0f,
            10f
        );
    }
}