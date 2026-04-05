using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreBoardUI : MonoBehaviour
{
    public GameObject rowPrefab;
    public Transform content;

    [Header("Rank Colors")]
    public Color goldColor;
    public Color silverColor;
    public Color bronzeColor;
    public Color normalColor = Color.white;

    void Start()
    {
        if (ScoreManager.instance == null)
        {
            Debug.LogError("ScoreManager chưa tồn tại!");
            return;
        }

        List<int> scores = ScoreManager.instance.GetScores();

        int rank = 1;

        foreach (int score in scores)
        {
            GameObject row = Instantiate(rowPrefab, content);

            // TEXT
            row.transform.Find("Rank")
                .GetComponent<TextMeshProUGUI>().text = rank + ".";

            row.transform.Find("Name")
                .GetComponent<TextMeshProUGUI>().text = "Player";

            row.transform.Find("ScoreBlock/Score")
                .GetComponent<TextMeshProUGUI>().text = score.ToString("N0");

            row.transform.Find("ScoreBlock/Date")
                .GetComponent<TextMeshProUGUI>().text = "-";

            // 🎨 LẤY IMAGE
            Image bg = row.GetComponent<Image>();

            if (bg != null)
            {
                if (rank == 1)
                    bg.color = goldColor;
                else if (rank == 2)
                    bg.color = silverColor;
                else if (rank == 3)
                    bg.color = bronzeColor;
                else
                    bg.color = normalColor;
            }

            rank++;
        }
    }
}