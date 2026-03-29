using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    private ShopItem currentItem;

    void Awake()
    {
        Instance = this;
    }

    public void SelectItem(ShopItem item)
    {
        // 👉 dừng con cũ
        if (currentItem != null && currentItem != item)
        {
            currentItem.StopJumpLoop();
        }

        // 👉 set con mới
        currentItem = item;

        // 👉 bắt đầu nhảy
        currentItem.StartJumpLoop();
    }
}