using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void BackToHome()
    {
        SceneManager.LoadScene("Home");
    }
    public void OpenSettings()
    {
        PlayerPrefs.SetString(
            "SettingsPreviousScene",
            SceneManager.GetActiveScene().name
        );

        PlayerPrefs.Save();

        SceneManager.LoadScene("Settings");
    }

    public void OpenModes()
    {
        SceneManager.LoadScene("Modes");
    }

    public void OpenTraining()
    {
        SceneManager.LoadScene("TrainingGround");
    }

    public void ExitTraining()
    {
        SceneManager.LoadScene("Modes");
    }
}