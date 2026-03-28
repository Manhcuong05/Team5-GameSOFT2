using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Over")]
    public GameObject gameOverUI;

    private bool isGameOver = false;

    void Awake()
    {
        Instance = this;
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        Debug.Log("GAME OVER");

        // Hiện UI
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        // Dừng game
        Time.timeScale = 0f;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}