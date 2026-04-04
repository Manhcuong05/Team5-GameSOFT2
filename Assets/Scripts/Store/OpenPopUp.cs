using UnityEngine;

public class OpenPopupUI : MonoBehaviour
{
    [Header("Popup cần mở")]
    public GameObject popup;

    public void OpenPopup()
    {
        Debug.Log("🛒 Open IAP Popup");

        popup.SetActive(true);

        // nếu bạn muốn pause game
        Time.timeScale = 0f;
    }
}