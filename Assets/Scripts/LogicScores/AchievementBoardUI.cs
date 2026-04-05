using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementBoardUI : MonoBehaviour
{
    public GameObject achievementRowPrefab;
    public Transform content;
    public TMP_Text unlockedSummaryText;
    public TMP_Text scoreText;

    void Start()
    {
        GenerateAchievements();
    }

    public void GenerateAchievements()
    {
        if (AchievementTracker.Instance == null)
        {
            Debug.LogError("AchievementTracker not found.");
            return;
        }

        if (achievementRowPrefab == null || content == null)
        {
            Debug.LogError("Missing prefab or content.");
            return;
        }

        ClearRows();

        List<AchievementTracker.AchievementData> list = AchievementTracker.Instance.GetAchievements();

        for (int i = 0; i < list.Count; i++)
        {
            GameObject rowObj = Instantiate(achievementRowPrefab, content);
            rowObj.transform.localScale = Vector3.one;

            AchievementRowUI rowUI = rowObj.GetComponent<AchievementRowUI>();
            if (rowUI != null)
            {
                rowUI.Setup(i + 1, list[i].title, list[i].unlocked);
            }
        }

        if (unlockedSummaryText != null)
            unlockedSummaryText.text = AchievementTracker.Instance.GetUnlockedCount() + "/" + list.Count;

        if (scoreText != null)
            scoreText.text = AchievementTracker.Instance.currentScore.ToString("N0");
    }

    void ClearRows()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
}