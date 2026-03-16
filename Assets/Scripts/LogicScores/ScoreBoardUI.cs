using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScoreBoardUI : MonoBehaviour
{
    public GameObject rowPrefab;
    public Transform content;

    void Start()
    {
        Debug.Log(rowPrefab);
        Debug.Log(content);
        Debug.Log(ScoreManager.instance);

        List<int> testScores = new List<int>()
        {
            26756453,
            15105565,
            7836545,
            687935,
            526745,
            526745,
            526745,
            526745,
            526745,
            526745,
            526745,
            475745
        };

        int rank = 1;

        foreach (int score in testScores)
        {
            GameObject row = Instantiate(rowPrefab, content);

            // Rank
            row.transform.Find("Rank")
                .GetComponent<TextMeshProUGUI>().text = rank.ToString() + ".";

            // Name
            row.transform.Find("Name")
                .GetComponent<TextMeshProUGUI>().text = "Bram";

            // Score
            row.transform.Find("ScoreBlock/Score")
                .GetComponent<TextMeshProUGUI>().text = score.ToString("N0");

            // Date
            row.transform.Find("ScoreBlock/Date")
                .GetComponent<TextMeshProUGUI>().text = System.DateTime.Now.ToString("MMM dd, yyyy");

            rank++;
        }
    }
}