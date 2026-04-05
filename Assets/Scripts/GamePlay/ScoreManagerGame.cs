using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManagerGame : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI scoreText;

    private float startY;
    private float highestY;
    private int score;

    void Start()
    {
        if (player != null)
        {
            startY = player.position.y;
            highestY = player.position.y;
            score = 0;

            UpdateUI();

            // 👉 reset score trong GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.currentScore = 0;
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        if (player.position.y > highestY)
        {
            highestY = player.position.y;

            float heightPassed = highestY - startY;

            score = Mathf.Max(0, Mathf.FloorToInt(heightPassed * 10f));

            UpdateUI();

            // 👉 QUAN TRỌNG: đồng bộ sang GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.currentScore = score;
            }
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    // 👉 cho GameManager lấy nếu cần
    public int GetScore()
    {
        return score;
    }
}