using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AndroidBackHandler : MonoBehaviour
{
    private void Update()
    {
        // This script is only for Android back behavior.
        // Do NOT use PC ESC here.
        if (!Application.isMobilePlatform)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HandleBack();
        }
    }

    private void HandleBack()
    {
        string currentScene =
            SceneManager.GetActiveScene().name;

        if (currentScene == "Home")
        {
            Application.Quit();
        }
        else if (currentScene == "Modes")
        {
            SceneManager.LoadScene("Home");
        }
        else if (currentScene == "TrainingGround")
        {
            SceneManager.LoadScene("Home");
        }
        else if (currentScene == "Settings")
        {
            string previousScene =
                PlayerPrefs.GetString(
                    "SettingsPreviousScene",
                    "Home"
                );

            SceneManager.LoadScene(previousScene);
        }
    }
}