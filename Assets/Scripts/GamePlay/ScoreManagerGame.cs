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
        }
    }

    void Update()
    {
        if (player == null) return;

        // chỉ cập nhật khi player lên cao hơn
        if (player.position.y > highestY)
        {
            highestY = player.position.y;

            // tính điểm theo độ cao tăng lên từ vị trí bắt đầu
            float heightPassed = highestY - startY;
            score = Mathf.Max(0, Mathf.FloorToInt(heightPassed * 10f));

            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
}