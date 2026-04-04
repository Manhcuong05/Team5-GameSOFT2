using UnityEngine;
using TMPro;

public class EggManager : MonoBehaviour
{
    public static EggManager instance;

    public TextMeshProUGUI eggText;

    private int eggs;

    private const string EggKey = "EGGS";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        LoadEggs();
    }

    public void AddEggs(int amount)
    {
        eggs += amount;
        if (eggs < 0) eggs = 0;

        SaveEggs();
        UpdateUI();
    }

    public void SetEggs(int amount)
    {
        eggs = Mathf.Max(0, amount);

        SaveEggs();
        UpdateUI();
    }

    public int GetEggs()
    {
        return eggs;
    }

    public bool SpendEggs(int amount)
    {
        if (eggs < amount)
            return false;

        eggs -= amount;
        SaveEggs();
        UpdateUI();
        return true;
    }

    private void LoadEggs()
    {
        eggs = PlayerPrefs.GetInt(EggKey, 0);
        UpdateUI();
    }

    private void SaveEggs()
    {
        PlayerPrefs.SetInt(EggKey, eggs);
        PlayerPrefs.Save();
    }

    private void UpdateUI()
    {
        if (eggText != null)
        {
            eggText.text = eggs.ToString();
        }

        if (ShopManager.Instance != null)
        {
            ShopManager.Instance.RefreshAllItems();
        }
    }
}