using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ResetOption : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text resetText;

    public Color normalColor = new Color(0.15f, 0.15f, 0.15f);
    public Color clickedColor = new Color(0.35f, 0.75f, 0.2f);

    private void Start()
    {
        UpdateColors(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UpdateColors(true);
        ResetScoreAndStats();
    }

    public void UpdateColors(bool clicked)
    {
        if (resetText != null)
            resetText.color = clicked ? clickedColor : normalColor;
    }

    private void ResetScoreAndStats()
    {
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
        }

        PlayerPrefs.Save();

        Debug.Log("ResetOption: Score + Stats reset complete");

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}