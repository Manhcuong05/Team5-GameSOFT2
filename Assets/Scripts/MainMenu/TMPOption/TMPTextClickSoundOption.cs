using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TMPTextClickSoundOption : MonoBehaviour, IPointerClickHandler
{
    [Header("Sound Option")]
    public SoundOption soundOption;
    public bool isOnText;

    [Header("Optional Button Action")]
    public Button targetButton;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("TMP clicked: " + gameObject.name);

        if (targetButton != null)
        {
            targetButton.onClick.Invoke();
            return;
        }

        if (soundOption == null) return;

        soundOption.isOnOption = isOnText;
        soundOption.OnPointerClick(eventData);
    }
}