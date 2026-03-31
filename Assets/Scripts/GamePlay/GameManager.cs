using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Over")]
    public GameObject gameOverUI;

    [Header("Score")]
    public int currentScore = 0;

    private bool isGameOver = false;

    void Awake()
    {
        Instance = this;
    }

    // =========================
    // SCORE
    // =========================
    public void AddScore(int amount)
    {
        if (isGameOver) return;

        currentScore += amount;
        Debug.Log("Score: " + currentScore);
    }

    public int GetScore()
    {
        return currentScore;
    }

    // =========================
    // GAME OVER
    // =========================
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        Debug.Log("GAME OVER - Score: " + currentScore);

        // 👉 lưu score từ ScoreManagerGame
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(currentScore);
        }

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Time.timeScale = 0f;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;
        currentScore = 0;
        isGameOver = false;
    }
}