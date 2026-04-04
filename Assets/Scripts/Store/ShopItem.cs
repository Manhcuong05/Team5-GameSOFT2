using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [Header("Jump")]
    public float jumpHeight = 1.5f;
    public float jumpDuration = 0.3f;

    [Header("Item Data")]
    public string itemName;
    public int price;

    [Header("State")]
    public bool canBuy = true;
    public bool isOwned = false;

    [Header("Price UI In Canvas")]
    public GameObject pricePanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI priceText;
    public Image pricePanelImage;

    private Vector3 startPos;
    private Coroutine jumpLoop;

    private string OwnedKey
    {
        get { return "SHOP_ITEM_OWNED_" + itemName; }
    }

    void Start()
    {
        startPos = transform.position;
        LoadOwnedState();

        if (pricePanel != null)
        {
            if (pricePanelImage == null)
                pricePanelImage = pricePanel.GetComponent<Image>();

            PricePanelClick clickHandler = pricePanel.GetComponent<PricePanelClick>();
            if (clickHandler == null)
                clickHandler = pricePanel.AddComponent<PricePanelClick>();

            pricePanel.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        Debug.Log("[ShopItem] Click item world: " + itemName);

        if (ShopManager.Instance != null)
        {
            ShopManager.Instance.SelectItem(this);
        }
    }

    public void StartJumpLoop()
    {
        if (jumpLoop == null)
            jumpLoop = StartCoroutine(JumpLoop());
    }

    public void StopJumpLoop()
    {
        if (jumpLoop != null)
        {
            StopCoroutine(jumpLoop);
            jumpLoop = null;
        }

        transform.position = startPos;
    }

    IEnumerator JumpLoop()
    {
        while (true)
        {
            float time = 0f;

            while (time < jumpDuration)
            {
                float t = time / jumpDuration;
                float y = Mathf.Sin(t * Mathf.PI) * jumpHeight;

                transform.position = startPos + Vector3.up * y;

                time += Time.deltaTime;
                yield return null;
            }

            transform.position = startPos;
            yield return null;
        }
    }

    public void ShowInfo(int currentEggs)
    {
        if (pricePanel != null)
            pricePanel.SetActive(true);

        RefreshState(currentEggs);
        UpdatePanelText();
        UpdateVisual(currentEggs);

        Debug.Log("[ShopItem] ShowInfo: " + itemName + " | Eggs = " + currentEggs + " | Owned = " + isOwned);
    }

    public void HideInfo()
    {
        if (pricePanel != null)
            pricePanel.SetActive(false);
    }

    public void RefreshState(int currentEggs)
    {
        canBuy = isOwned || currentEggs >= price;
    }

    public void MarkAsOwned()
    {
        isOwned = true;
        SaveOwnedState();

        Debug.Log("[ShopItem] MarkAsOwned: " + itemName);
    }

    public void UpdatePanelText()
    {
        if (itemNameText != null)
            itemNameText.text = itemName;

        if (priceText != null)
            priceText.text = isOwned ? "PLAY" : price.ToString();
    }

    public void UpdateVisual(int currentEggs)
    {
        bool enough = isOwned || currentEggs >= price;
        float alpha = enough ? 1f : 0.35f;

        if (pricePanelImage != null)
        {
            Color c = pricePanelImage.color;
            c.a = alpha;
            pricePanelImage.color = c;
        }

        if (itemNameText != null)
        {
            Color c = itemNameText.color;
            c.a = alpha;
            itemNameText.color = c;
        }

        if (priceText != null)
        {
            Color c = priceText.color;
            c.a = alpha;
            priceText.color = c;
        }
    }

    private void SaveOwnedState()
    {
        PlayerPrefs.SetInt(OwnedKey, isOwned ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadOwnedState()
    {
        isOwned = PlayerPrefs.GetInt(OwnedKey, 0) == 1;
        Debug.Log("[ShopItem] LoadOwnedState: " + itemName + " = " + isOwned);
    }

    public void ReloadOwnedState()
    {
        LoadOwnedState();
    }
}