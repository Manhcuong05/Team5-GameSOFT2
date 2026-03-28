using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManagerGame : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI scoreText;

    private float highestY;
    private int score;

    void Start()
    {
        if (player != null)
        {
            highestY = player.position.y;
        }
    }

    void Update()
    {
        if (player == null) return;

        // chỉ cập nhật khi player lên cao hơn
        if (player.position.y > highestY)
        {
            highestY = player.position.y;

            // scale điểm (tùy chỉnh)
            score = Mathf.FloorToInt(highestY * 10);

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
