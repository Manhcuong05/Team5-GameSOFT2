using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    private ShopItem currentItem;

    [Header("All Shop Items")]
    public ShopItem[] allItems;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ReloadAllOwnedStates();
        RefreshAllItems();
    }

    public void SelectItem(ShopItem item)
    {
        if (item == null) return;

        if (currentItem != null && currentItem != item)
        {
            currentItem.StopJumpLoop();
            currentItem.HideInfo();
        }

        currentItem = item;
        currentItem.StartJumpLoop();
        currentItem.ShowInfo(GetCurrentEggs());

        Debug.Log("[ShopManager] Selected item: " + currentItem.itemName);
    }

    public int GetCurrentEggs()
    {
        if (EggManager.instance == null)
        {
            Debug.LogWarning("EggManager.instance is NULL");
            return 0;
        }

        return EggManager.instance.GetEggs();
    }

    public void RefreshAllItems()
    {
        int eggs = GetCurrentEggs();

        if (allItems == null) return;

        for (int i = 0; i < allItems.Length; i++)
        {
            if (allItems[i] == null) continue;
            allItems[i].RefreshState(eggs);
        }

        if (currentItem != null)
        {
            currentItem.ShowInfo(eggs);
        }
    }

    public void ReloadAllOwnedStates()
    {
        if (allItems == null) return;

        for (int i = 0; i < allItems.Length; i++)
        {
            if (allItems[i] != null)
                allItems[i].ReloadOwnedState();
        }
    }

    public void ForceRefreshCurrentItemUI()
    {
        if (currentItem != null)
        {
            currentItem.ShowInfo(GetCurrentEggs());
        }
    }

    public bool BuyOrPlayCurrentItem()
    {
        if (currentItem == null)
        {
            Debug.Log("Chưa chọn item nào");
            return false;
        }

        if (currentItem.isOwned)
        {
            Debug.Log("PLAY item: " + currentItem.itemName);
            PlayerPrefs.SetString("SELECTED_ITEM", currentItem.itemName);
            PlayerPrefs.Save();
            return true;
        }

        if (EggManager.instance == null)
        {
            Debug.LogWarning("Không tìm thấy EggManager");
            return false;
        }

        bool success = EggManager.instance.SpendEggs(currentItem.price);

        if (success)
        {
            currentItem.MarkAsOwned();
            RefreshAllItems();

            Debug.Log("Mua thành công: " + currentItem.itemName);
            return true;
        }
        else
        {
            Debug.Log("Không đủ trứng để mua: " + currentItem.itemName);
            currentItem.ShowInfo(GetCurrentEggs());
            return false;
        }
    }
}