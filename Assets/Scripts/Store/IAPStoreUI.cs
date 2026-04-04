using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IAPItemUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI")]
    public Image image;
    public Sprite normalSprite;
    public Sprite selectedSprite;

    [Header("DATA")]
    public int eggAmount = 100;
    public bool isFreeAds = false;

    private static IAPItemUI currentSelected;

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectItem();

        if (isFreeAds)
        {
            IAPRewardADS.instance.ShowAd(OnRewardSuccess);
        }
        else
        {
            AddEggs(eggAmount);
        }
    }

    void SelectItem()
    {
        if (currentSelected != null && currentSelected != this)
        {
            currentSelected.image.sprite = currentSelected.normalSprite;
            currentSelected.transform.localScale = Vector3.one;
        }

        image.sprite = selectedSprite;
        currentSelected = this;

        transform.localScale = Vector3.one * 1.1f;
    }

    void OnRewardSuccess()
    {
        AddEggs(eggAmount);
    }

    void AddEggs(int amount)
    {
        if (EggManager.instance != null)
        {
            EggManager.instance.AddEggs(amount);
        }
    }
}