using UnityEngine;
using UnityEngine.EventSystems;

public class PricePanelClick : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("[PricePanelClick] Click vào khung tím");

        if (ShopManager.Instance == null)
        {
            Debug.LogWarning("[PricePanelClick] ShopManager.Instance NULL");
            return;
        }

        bool result = ShopManager.Instance.BuyOrPlayCurrentItem();
        Debug.Log("[PricePanelClick] BuyOrPlayCurrentItem result = " + result);
    }
}