using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    [Header("Settings UI")]
    public UnityEngine.UI.Slider sensitivitySlider;
    public UnityEngine.UI.Slider volumeSlider;

    [Header("Default Values")]
    public float defaultSensitivity = 0.1f;
    public float defaultVolume = 1f;

    private void Start()
    {
        LoadSettings();

        if (sensitivitySlider != null)
        {
            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    // =====================================================
    // SENSITIVITY
    // =====================================================

    public void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat(
            "Sensitivity",
            value
        );

        PlayerPrefs.Save();
    }

    // =====================================================
    // MASTER VOLUME
    // =====================================================

    public void SetVolume(float value)
    {
        PlayerPrefs.SetFloat(
            "MasterVolume",
            value
        );

        AudioListener.volume = value;

        PlayerPrefs.Save();
    }

    // =====================================================
    // LOAD SETTINGS
    // =====================================================

    private void LoadSettings()
    {
        float sensitivity =
            PlayerPrefs.GetFloat(
                "Sensitivity",
                defaultSensitivity
            );

        float volume =
            PlayerPrefs.GetFloat(
                "MasterVolume",
                defaultVolume
            );

        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = sensitivity;
        }

        if (volumeSlider != null)
        {
            volumeSlider.value = volume;
        }

        AudioListener.volume = volume;
    }
    public void Back()
    {
        string previousScene =
            PlayerPrefs.GetString(
                "SettingsPreviousScene",
                "Home"
            );

        SceneManager.LoadScene(previousScene);
    }
}