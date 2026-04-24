using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Volume")]
    public Slider volumeSlider;
    public TextMeshProUGUI volumeText;

    [Header("Mouse Sensitivity")]
    public Slider sensitivitySlider;
    public TextMeshProUGUI sensitivityText;

    [Header("Panel")]
    public GameObject settingsPanel;

    // Static values so other scripts can access them
    public static float masterVolume = 1f;
    public static float mouseSensitivity = 100f;

    void Start()
    {
        // Load saved settings FIRST
        masterVolume = PlayerPrefs.GetFloat("masterVolume", 1f);
        mouseSensitivity = PlayerPrefs.GetFloat("mouseSensitivity", 100f);

        // THEN set sliders to loaded values
        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = masterVolume;
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = 10f;
            sensitivitySlider.maxValue = 200f;
            sensitivitySlider.value = mouseSensitivity; // This will now show 100 correctly!
        }

        ApplyVolume();
        UpdateUI();

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // Save when closing
        SaveSettings();
    }

    public void OnVolumeChanged()
    {
        masterVolume = volumeSlider.value;
        ApplyVolume();
        UpdateUI();
    }

    public void OnSensitivityChanged()
    {
        mouseSensitivity = sensitivitySlider.value;
        UpdateUI();
    }

    void ApplyVolume()
    {
        AudioListener.volume = masterVolume;
    }

    void UpdateUI()
    {
        if (volumeText != null)
            volumeText.text = Mathf.RoundToInt(masterVolume * 100) + "%";

        if (sensitivityText != null)
            sensitivityText.text = mouseSensitivity.ToString("F1");
    }

    void SaveSettings()
    {
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
        PlayerPrefs.SetFloat("mouseSensitivity", mouseSensitivity);
        PlayerPrefs.Save();
    }
}