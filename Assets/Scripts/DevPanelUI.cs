using UnityEngine;
using UnityEngine.SceneManagement;

public class DevPanelUI : MonoBehaviour
{
    public GameObject devPopup;
    public bool enableDevMode = true;

    [Header("Shop Item Names")]
    public string[] shopItemNames;

    public void OpenDevPanel()
    {
        if (!enableDevMode) return;
        if (devPopup == null) return;

        devPopup.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseDevPanel()
    {
        if (devPopup != null)
            devPopup.SetActive(false);

        Time.timeScale = 1f;
    }

    public void ResetAllData()
    {
        if (!enableDevMode) return;

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetEggs()
    {
        if (!enableDevMode) return;

        PlayerPrefs.SetInt("EGGS", 0);
        PlayerPrefs.Save();

        if (EggManager.instance != null)
        {
            EggManager.instance.SetEggs(0);
        }

        Debug.Log("DevMode: EGGS reset to 0");
    }

    public void ResetShopOwnedItems()
    {
        if (!enableDevMode) return;

        if (shopItemNames != null)
        {
            for (int i = 0; i < shopItemNames.Length; i++)
            {
                if (string.IsNullOrEmpty(shopItemNames[i])) continue;

                string ownedKey = "SHOP_ITEM_OWNED_" + shopItemNames[i];
                PlayerPrefs.DeleteKey(ownedKey);
                Debug.Log("DevMode: Deleted key -> " + ownedKey);
            }
        }

        PlayerPrefs.DeleteKey("SELECTED_ITEM");
        PlayerPrefs.Save();

        if (ShopManager.Instance != null)
        {
            ShopManager.Instance.ReloadAllOwnedStates();
            ShopManager.Instance.RefreshAllItems();
            ShopManager.Instance.ForceRefreshCurrentItemUI();
        }

        Debug.Log("DevMode: Reset tất cả trạng thái đã mua item shop");
    }

    public void ResetScoreAndStats()
    {
        if (!enableDevMode) return;

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.scores.Clear();
        }

        PlayerPrefs.DeleteKey("SCORE_DATA");

        if (GameStatsTracker.instance != null)
        {
            GameStatsTracker.instance.ResetStats();
        }
        else
        {
            PlayerPrefs.DeleteKey("TotalGames");
            PlayerPrefs.DeleteKey("HighScore");
            PlayerPrefs.DeleteKey("LastScore");
            PlayerPrefs.DeleteKey("TotalScore");
            PlayerPrefs.DeleteKey("TotalPlayTime");
            PlayerPrefs.DeleteKey("LongestPlayTime");
            PlayerPrefs.DeleteKey("LastPlayTime");
            PlayerPrefs.Save();
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}