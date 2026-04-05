using UnityEngine;
using UnityEngine.EventSystems;

public class IAPPopupUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Popup cần đóng")]
    public GameObject popup;

    public void OnPointerClick(PointerEventData eventData)
    {
        ClosePopup();
    }

    public void ClosePopup()
    {
        Debug.Log("❌ Close IAP Popup");

        popup.SetActive(false);

        // nếu bạn có pause game
        Time.timeScale = 1f;
    }
}