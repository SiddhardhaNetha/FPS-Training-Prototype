using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    public int magazineSize = 30;
    public int reserveAmmo = 90;
    public int currentAmmo;

    public float damage = 25f;
    public float fireRate = 10f;
    public float range = 100f;

    [Header("Reload")]
    public float reloadTime = 2f;

    [Header("References")]
    public Camera playerCamera;
    public TMP_Text ammoText;
    public PlayerController playerController;
    public HitMarker hitMarker;
    public TargetPracticeManager practiceManager;

    [Header("Mobile Controls")]
    public MobileFireButton fireButton;

    [Header("Muzzle Flash")]
    public ParticleSystem muzzleFlash;

    [Header("Audio")]
    public AudioSource fireAudio;
    public AudioSource reloadAudio;
    public AudioSource hitAudio;

    private float nextFireTime;
    private bool isReloading;

    private void Start()
    {
        currentAmmo = magazineSize;

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        UpdateAmmoUI();
    }

    private void Update()
    {
        // =============================================
        // PC RELOAD
        // =============================================

        if (Keyboard.current != null &&
            Keyboard.current.rKey.wasPressedThisFrame)
        {
            StartReload();
        }

        // =============================================
        // DON'T SHOOT WHILE RELOADING
        // =============================================

        if (isReloading)
            return;

        // =============================================
        // PC FIRE
        // =============================================

        bool pcFire =
            Mouse.current != null &&
            Mouse.current.leftButton.isPressed &&
            !IsPointerOverUI();

        // =============================================
        // MOBILE FIRE
        // =============================================

        bool mobileFire =
            fireButton != null &&
            fireButton.IsPressed;

        if (pcFire || mobileFire)
        {
            Fire();
        }
    }

    // =========================================================
    // UI CHECK
    // =========================================================

    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;

        return EventSystem.current.IsPointerOverGameObject();
    }

    // =========================================================
    // FIRE
    // =========================================================

    private void Fire()
    {
        // Magazine empty
        if (currentAmmo <= 0)
        {
            Debug.Log("Magazine empty. Auto reloading...");

            StartReload();

            return;
        }

        // Fire rate
        if (Time.time < nextFireTime)
            return;

        nextFireTime =
            Time.time + (1f / fireRate);

        // =============================================
        // CONSUME AMMO
        // =============================================

        currentAmmo--;

        UpdateAmmoUI();

        if (practiceManager != null)
        {
            practiceManager.RegisterShot();
        }

        // =============================================
        // MUZZLE FLASH
        // =============================================

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // =============================================
        // FIRE SOUND
        // =============================================

        if (fireAudio != null)
        {
            fireAudio.Play();
        }

        // =============================================
        // CAMERA RECOIL
        // =============================================

        if (playerController != null)
        {
            playerController.AddRecoil();
        }

        // =============================================
        // CREATE RAY
        // =============================================

        Ray ray =
            playerCamera.ViewportPointToRay(
                new Vector3(
                    0.5f,
                    0.5f,
                    0f
                )
            );

        // =============================================
        // SHOOT
        // =============================================

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            range))
        {
            Debug.Log(
                "SHOT! Hit: " +
                hit.collider.name +
                " | Ammo: " +
                currentAmmo +
                " / " +
                reserveAmmo
            );

            TargetDummy target =
                hit.collider.GetComponent<TargetDummy>();

            if (target != null)
            {
                target.TakeDamage(damage);

                if (practiceManager != null)
                {
                    practiceManager.RegisterHit();
                }

                if (hitMarker != null)
                {
                    hitMarker.Show();
                }

                if (hitAudio != null)
                {
                    hitAudio.Play();
                }
            }
        }
        else
        {
            Debug.Log(
                "SHOT! Miss | Ammo: " +
                currentAmmo +
                " / " +
                reserveAmmo
            );
        }

        // =============================================
        // AUTO RELOAD WHEN MAGAZINE BECOMES EMPTY
        // =============================================

        if (currentAmmo <= 0)
        {
            Debug.Log("Magazine empty. Starting auto reload.");

            StartReload();
        }
    }

    // =========================================================
    // MOBILE RELOAD
    // =========================================================

    public void MobileReload()
    {
        StartReload();
    }

    // =========================================================
    // RELOAD
    // =========================================================

    private void StartReload()
    {
        // Already reloading
        if (isReloading)
            return;

        // Magazine full
        if (currentAmmo >= magazineSize)
        {
            Debug.Log("Magazine already full.");
            return;
        }

        // No reserve ammo
        if (reserveAmmo <= 0)
        {
            Debug.Log("No reserve ammo.");
            return;
        }

        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        Debug.Log("Reloading...");

        // =============================================
        // RELOAD SOUND
        // =============================================

        if (reloadAudio != null)
        {
            reloadAudio.Play();
        }

        // =============================================
        // WAIT FOR RELOAD
        // =============================================

        yield return new WaitForSeconds(reloadTime);

        // =============================================
        // CALCULATE AMMO
        // =============================================

        int ammoNeeded =
            magazineSize - currentAmmo;

        int ammoToLoad =
            Mathf.Min(
                ammoNeeded,
                reserveAmmo
            );

        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;

        isReloading = false;

        UpdateAmmoUI();

        Debug.Log(
            "Reload complete! Ammo: " +
            currentAmmo +
            " / " +
            reserveAmmo
        );
    }

    // =========================================================
    // AMMO UI
    // =========================================================

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text =
                currentAmmo +
                " / " +
                reserveAmmo;
        }
    }
}