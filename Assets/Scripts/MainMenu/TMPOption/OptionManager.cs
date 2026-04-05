using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class OptionManager : MonoBehaviour
{
    public static OptionManager Instance;

    [Header("Sound")]
    public Toggle soundOffToggle;
    public Toggle soundOnToggle;
    public TMP_Text soundOnText;
    public TMP_Text soundOffText;

    [Header("Calibrate")]
    public Toggle calibrateAutoToggle;
    public Toggle calibrateManualToggle;

    [Header("Colors")]
    public Color selectedColor = new Color(0.35f, 0.75f, 0.2f);
    public Color unselectedColor = new Color(0.15f, 0.15f, 0.15f);

    private const string SOUND_KEY = "sound_enabled";
    private const string CALIBRATE_MODE_KEY = "calibrate_mode";

    public bool SoundEnabled { get; private set; }
    public bool IsManualCalibrate { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadSettings();
        ApplySettingsToUI();
        ApplyRuntimeSettings();
        BindEvents();
        UpdateSoundTextColors();
    }

    private void LoadSettings()
    {
        SoundEnabled = PlayerPrefs.GetInt(SOUND_KEY, 1) == 1;
        IsManualCalibrate = PlayerPrefs.GetInt(CALIBRATE_MODE_KEY, 0) == 1;
    }

    private void BindEvents()
    {
        if (soundOffToggle != null)
            soundOffToggle.onValueChanged.AddListener(OnSoundOffChanged);

        if (soundOnToggle != null)
            soundOnToggle.onValueChanged.AddListener(OnSoundOnChanged);

        if (calibrateAutoToggle != null)
            calibrateAutoToggle.onValueChanged.AddListener(OnCalibrateAutoChanged);

        if (calibrateManualToggle != null)
            calibrateManualToggle.onValueChanged.AddListener(OnCalibrateManualChanged);
    }

    private void ApplySettingsToUI()
    {
        if (soundOnToggle != null) soundOnToggle.isOn = SoundEnabled;
        if (soundOffToggle != null) soundOffToggle.isOn = !SoundEnabled;

        if (calibrateManualToggle != null) calibrateManualToggle.isOn = IsManualCalibrate;
        if (calibrateAutoToggle != null) calibrateAutoToggle.isOn = !IsManualCalibrate;
    }

    private void ApplyRuntimeSettings()
    {
        AudioListener.volume = SoundEnabled ? 1f : 0f;
    }

    private void UpdateSoundTextColors()
    {
        if (soundOnText != null)
            soundOnText.color = SoundEnabled ? selectedColor : unselectedColor;

        if (soundOffText != null)
            soundOffText.color = SoundEnabled ? unselectedColor : selectedColor;
    }

    private void OnSoundOffChanged(bool isOn)
    {
        if (!isOn) return;

        SoundEnabled = false;
        PlayerPrefs.SetInt(SOUND_KEY, 0);
        PlayerPrefs.Save();

        if (soundOnToggle != null && soundOnToggle.isOn)
            soundOnToggle.isOn = false;

        ApplyRuntimeSettings();
        UpdateSoundTextColors();
    }

    private void OnSoundOnChanged(bool isOn)
    {
        if (!isOn) return;

        SoundEnabled = true;
        PlayerPrefs.SetInt(SOUND_KEY, 1);
        PlayerPrefs.Save();

        if (soundOffToggle != null && soundOffToggle.isOn)
            soundOffToggle.isOn = false;

        ApplyRuntimeSettings();
        UpdateSoundTextColors();
    }

    private void OnCalibrateAutoChanged(bool isOn)
    {
        if (!isOn) return;

        IsManualCalibrate = false;
        PlayerPrefs.SetInt(CALIBRATE_MODE_KEY, 0);
        PlayerPrefs.Save();

        if (calibrateManualToggle != null && calibrateManualToggle.isOn)
            calibrateManualToggle.isOn = false;
    }

    private void OnCalibrateManualChanged(bool isOn)
    {
        if (!isOn) return;

        IsManualCalibrate = true;
        PlayerPrefs.SetInt(CALIBRATE_MODE_KEY, 1);
        PlayerPrefs.Save();

        if (calibrateAutoToggle != null && calibrateAutoToggle.isOn)
            calibrateAutoToggle.isOn = false;
    }
}