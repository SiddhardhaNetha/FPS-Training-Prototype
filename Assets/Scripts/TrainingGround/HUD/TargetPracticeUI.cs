using UnityEngine;
using TMPro;

public class TargetPracticeUI : MonoBehaviour
{
    [Header("References")]
    public TargetPracticeManager practiceManager;
    public TMP_Text statsText;

    private void Update()
    {
        if (practiceManager == null ||
            statsText == null)
            return;

        int remaining =
            practiceManager.GetRemainingTargets();

        int total =
            practiceManager.GetTotalTargets();

        int shots =
            practiceManager.shotsFired;

        int hits =
            practiceManager.targetsHit;

        float accuracy =
            practiceManager.GetAccuracy();

        statsText.text =
            "TARGETS: " +
            remaining +
            " / " +
            total +
            "\n" +
            "SHOTS: " +
            shots +
            "\n" +
            "HITS: " +
            hits +
            "\n" +
            "ACCURACY: " +
            accuracy.ToString("F1") +
            "%";
    }
}