using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler,
    IPointerUpHandler
{
    [Header("References")]
    public RectTransform joystick;
    public RectTransform handle;

    [Tooltip("The minimap area that should NOT activate the joystick.")]
    public RectTransform miniMap;

    [Header("Settings")]
    public float radius = 100f;

    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    private RectTransform movementArea;
    private int activePointerId = -1;

    // Original joystick position from the Inspector
    private Vector2 originalJoystickPosition;

    private void Awake()
    {
        movementArea = GetComponent<RectTransform>();

        Horizontal = 0f;
        Vertical = 0f;

        // Remember the joystick's original Inspector position
        if (joystick != null)
        {
            originalJoystickPosition = joystick.anchoredPosition;
        }

        if (handle != null)
        {
            handle.anchoredPosition = Vector2.zero;
        }
    }

    // =========================================================
    // CHECK IF TOUCH IS ON MINIMAP
    // =========================================================

    private bool IsTouchOnMiniMap(PointerEventData eventData)
    {
        if (miniMap == null)
            return false;

        return RectTransformUtility.RectangleContainsScreenPoint(
            miniMap,
            eventData.position,
            eventData.pressEventCamera
        );
    }

    // =========================================================
    // FINGER TOUCHES LEFT SIDE
    // =========================================================

    public void OnPointerDown(PointerEventData eventData)
    {
        // Do NOT activate joystick if the touch is on the minimap
        if (IsTouchOnMiniMap(eventData))
        {
            return;
        }

        if (activePointerId != -1)
            return;

        activePointerId = eventData.pointerId;

        // Move joystick center directly under the finger
        MoveJoystickToTouch(eventData);

        if (handle != null)
        {
            handle.anchoredPosition = Vector2.zero;
        }

        Horizontal = 0f;
        Vertical = 0f;
    }

    // =========================================================
    // MOVE JOYSTICK TO FINGER
    // =========================================================

    private void MoveJoystickToTouch(
        PointerEventData eventData)
    {
        if (joystick == null || movementArea == null)
            return;

        Vector3 worldPosition;

        bool converted =
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                movementArea,
                eventData.position,
                eventData.pressEventCamera,
                out worldPosition
            );

        if (converted)
        {
            // Put the CENTER of the joystick
            // exactly where the finger touched.
            joystick.position = worldPosition;
        }

        if (handle != null)
        {
            handle.anchoredPosition = Vector2.zero;
        }
    }

    // =========================================================
    // FINGER DRAGS
    // =========================================================

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != activePointerId)
            return;

        if (joystick == null || handle == null)
            return;

        Vector2 localPosition;

        bool converted =
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystick,
                eventData.position,
                eventData.pressEventCamera,
                out localPosition
            );

        if (!converted)
            return;

        // Limit handle movement
        localPosition =
            Vector2.ClampMagnitude(
                localPosition,
                radius
            );

        handle.anchoredPosition = localPosition;

        // Convert handle position into movement values
        Horizontal =
            localPosition.x / radius;

        Vertical =
            localPosition.y / radius;
    }

    // =========================================================
    // FINGER RELEASE
    // =========================================================

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId != activePointerId)
            return;

        activePointerId = -1;

        Horizontal = 0f;
        Vertical = 0f;

        if (handle != null)
        {
            handle.anchoredPosition = Vector2.zero;
        }

        // Return joystick to its original Inspector position
        if (joystick != null)
        {
            joystick.anchoredPosition = originalJoystickPosition;
        }
    }

    // =========================================================
    // DISABLE
    // =========================================================

    private void OnDisable()
    {
        activePointerId = -1;

        Horizontal = 0f;
        Vertical = 0f;

        if (handle != null)
        {
            handle.anchoredPosition = Vector2.zero;
        }

        // Return joystick to its original position
        if (joystick != null)
        {
            joystick.anchoredPosition = originalJoystickPosition;
        }
    }
}