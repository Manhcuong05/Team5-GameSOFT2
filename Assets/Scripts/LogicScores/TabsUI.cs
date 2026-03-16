using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabsUI : MonoBehaviour
{
    public Text scoresText;
    public Text statsText;
    public Text achievementsText;

    public Button scoresButton;

    public GameObject scorePanel;
    public GameObject statPanel;
    public GameObject achievementPanel;

    Color cherryRed = new Color32(194, 24, 7, 255);
    Color normalColor = Color.black;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(scoresButton.gameObject);

        scoresText.color = cherryRed;
        SelectScores();
    }

    public void SelectScores()
    {
        scoresText.color = cherryRed;
        statsText.color = normalColor;
        achievementsText.color = normalColor;

        scorePanel.SetActive(true);
        statPanel.SetActive(false);
        achievementPanel.SetActive(false);
    }

    public void SelectStats()
    {
        scoresText.color = normalColor;
        statsText.color = cherryRed;
        achievementsText.color = normalColor;

        scorePanel.SetActive(false);
        statPanel.SetActive(true);
        achievementPanel.SetActive(false);
    }

    public void SelectAchievements()
    {
        scoresText.color = normalColor;
        statsText.color = normalColor;
        achievementsText.color = cherryRed;

        scorePanel.SetActive(false);
        statPanel.SetActive(false);
        achievementPanel.SetActive(true);
    }
}