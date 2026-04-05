using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class SoundOption : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text onText;
    public TMP_Text offText;
    public bool isOnOption;

    public Color selectedColor = new Color(0.35f, 0.75f, 0.2f);
    public Color unselectedColor = new Color(0.15f, 0.15f, 0.15f);

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GlobalSoundApplier.Instance == null) return;

        GlobalSoundApplier.Instance.SetSound(isOnOption);
        UpdateColors();
    }

    private void Start()
    {
        UpdateColors();
    }

    public void UpdateColors()
    {
        if (GlobalSoundApplier.Instance == null) return;

        bool soundEnabled = GlobalSoundApplier.Instance.SoundEnabled;

        if (onText != null)
            onText.color = soundEnabled ? selectedColor : unselectedColor;

        if (offText != null)
            offText.color = soundEnabled ? unselectedColor : selectedColor;
    }
}