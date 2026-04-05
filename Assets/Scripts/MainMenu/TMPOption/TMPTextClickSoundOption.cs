using UnityEngine;
using UnityEngine.EventSystems;

public class TMPTextClickSoundOption : MonoBehaviour, IPointerClickHandler
{
    public SoundOption soundOption;
    public bool isOnText;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("TMP clicked: " + gameObject.name);

        if (soundOption == null) return;

        soundOption.isOnOption = isOnText;
        soundOption.OnPointerClick(eventData);
    }
}