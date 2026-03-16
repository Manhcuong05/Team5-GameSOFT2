using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsBoardUI : MonoBehaviour
{
    public GameObject statRowPrefab;
    public Transform content;

    void Start()
    {
        GenerateStats();
    }

    void GenerateStats()
    {
        CreateRow("Total games played", PlayerPrefs.GetInt("TotalGames", 0).ToString());

        CreateRow("High score", PlayerPrefs.GetInt("HighScore", 0).ToString("N0"));

        CreateRow("Last score", PlayerPrefs.GetInt("LastScore", 0).ToString("N0"));

        int games = PlayerPrefs.GetInt("TotalGames", 0);
        int totalScore = PlayerPrefs.GetInt("TotalScore", 0);

        int avg = games > 0 ? totalScore / games : 0;

        CreateRow("Average score", avg.ToString("N0"));

        CreateRow("Total play time", FormatTime(PlayerPrefs.GetFloat("TotalPlayTime", 0)));

        CreateRow("Longest play time", FormatTime(PlayerPrefs.GetFloat("LongestPlayTime", 0)));

        CreateRow("Last play time", FormatTime(PlayerPrefs.GetFloat("LastPlayTime", 0)));
    }

    void CreateRow(string label, string value)
    {
        GameObject row = Instantiate(statRowPrefab, content);

        row.transform.Find("Label")
            .GetComponent<TextMeshProUGUI>().text = label;

        row.transform.Find("Value")
            .GetComponent<TextMeshProUGUI>().text = value;
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        if (minutes > 0)
            return minutes + "m:" + seconds + "s";
        else
            return seconds + "s";
    }
}