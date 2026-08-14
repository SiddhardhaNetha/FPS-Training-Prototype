using UnityEngine;

public class TrainingResetManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public CharacterController playerController;
    public Gun gun;
    public PlayerHealth playerHealth;
    public TargetPracticeManager targetPracticeManager;

    [Header("Player Start")]
    public Transform playerStartPoint;

    private void Start()
    {
        RestartTraining();
    }

    // =========================================================
    // RESTART ENTIRE TRAINING
    // =========================================================

    public void RestartTraining()
    {
        ResetPlayer();
        ResetHealth();
        ResetAmmo();
        ResetTargets();
        ResetStats();

        Debug.Log("TRAINING RESTARTED!");
    }

    // =========================================================
    // RESET TARGETS ONLY
    // =========================================================

    public void ResetTargetsOnly()
    {
        if (targetPracticeManager == null)
            return;

        targetPracticeManager.FindTargets();

        foreach (TargetDummy target in targetPracticeManager.targets)
        {
            if (target != null)
            {
                target.ResetTarget();
            }
        }

        Debug.Log("TARGETS RESET! STATS KEPT.");
    }

    // =========================================================
    // RESET PLAYER POSITION
    // =========================================================

    private void ResetPlayer()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        if (player != null && playerStartPoint != null)
        {
            player.position = playerStartPoint.position;
            player.rotation = playerStartPoint.rotation;
        }

        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }

    // =========================================================
    // RESET PLAYER HEALTH
    // =========================================================

    private void ResetHealth()
    {
        if (playerHealth != null)
        {
            playerHealth.currentHealth =
                playerHealth.maxHealth;

            playerHealth.SendMessage(
                "UpdateHealthUI",
                SendMessageOptions.DontRequireReceiver
            );
        }
    }

    // =========================================================
    // RESET AMMO
    // =========================================================

    private void ResetAmmo()
    {
        if (gun != null)
        {
            gun.currentAmmo = gun.magazineSize;
            gun.reserveAmmo = 90;

            gun.SendMessage(
                "UpdateAmmoUI",
                SendMessageOptions.DontRequireReceiver
            );
        }
    }

    // =========================================================
    // RESET TARGETS FOR FULL RESTART
    // =========================================================

    private void ResetTargets()
    {
        if (targetPracticeManager == null)
            return;

        targetPracticeManager.FindTargets();

        foreach (TargetDummy target in targetPracticeManager.targets)
        {
            if (target != null)
            {
                target.ResetTarget();
            }
        }
    }

    // =========================================================
    // RESET PRACTICE STATS
    // =========================================================

    private void ResetStats()
    {
        if (targetPracticeManager != null)
        {
            targetPracticeManager.shotsFired = 0;
            targetPracticeManager.targetsHit = 0;
        }
    }
}