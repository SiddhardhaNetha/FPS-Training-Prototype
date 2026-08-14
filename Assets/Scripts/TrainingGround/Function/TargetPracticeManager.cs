using UnityEngine;

public class TargetPracticeManager : MonoBehaviour
{
    [Header("Targets")]
    public TargetDummy[] targets;

    [Header("Practice Stats")]
    public int shotsFired;
    public int targetsHit;

    private void Start()
    {
        FindTargets();

        Debug.Log(
            "Target Practice started. Targets found: " +
            targets.Length
        );
    }

    // =========================================================
    // FIND TARGETS
    // =========================================================

    public void FindTargets()
    {
        targets = FindObjectsByType<TargetDummy>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );
    }

    // =========================================================
    // RESET ALL TARGETS
    // =========================================================

    public void ResetAllTargets()
    {
        if (targets == null)
        {
            FindTargets();
        }

        foreach (TargetDummy target in targets)
        {
            if (target != null)
            {
                target.ResetTarget();
            }
        }

        shotsFired = 0;
        targetsHit = 0;

        Debug.Log("ALL TARGETS RESET!");
    }

    // =========================================================
    // TARGET COUNT
    // =========================================================

    public int GetTotalTargets()
    {
        if (targets == null)
            return 0;

        return targets.Length;
    }

    public int GetRemainingTargets()
    {
        if (targets == null)
            return 0;

        int remaining = 0;

        foreach (TargetDummy target in targets)
        {
            if (target != null &&
                target.gameObject.activeSelf)
            {
                remaining++;
            }
        }

        return remaining;
    }

    // =========================================================
    // SHOTS
    // =========================================================

    public void RegisterShot()
    {
        shotsFired++;
    }

    // =========================================================
    // HITS
    // =========================================================

    public void RegisterHit()
    {
        targetsHit++;
    }

    // =========================================================
    // ACCURACY
    // =========================================================

    public float GetAccuracy()
    {
        if (shotsFired <= 0)
            return 0f;

        return ((float)targetsHit / shotsFired) * 100f;
    }
}