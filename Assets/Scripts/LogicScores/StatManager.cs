using UnityEngine;

public class GameStatsTracker : MonoBehaviour
{
    private float playTime = 0f;
    private bool isPlaying = false;
    public static GameStatsTracker instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (isPlaying)
        {
            playTime += Time.deltaTime;
        }
    }

    public void StartGame()
    {
        playTime = 0f;
        isPlaying = true;
    }

    public void EndGame(int score)
    {
        isPlaying = false;

        // ===== LOAD =====
        int totalGames = PlayerPrefs.GetInt("TotalGames", 0);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        int totalScore = PlayerPrefs.GetInt("TotalScore", 0);

        float totalPlayTime = PlayerPrefs.GetFloat("TotalPlayTime", 0);
        float longestPlayTime = PlayerPrefs.GetFloat("LongestPlayTime", 0);

        // ===== UPDATE =====
        totalGames++;
        totalScore += score;

        if (score > highScore)
            highScore = score;

        if (playTime > longestPlayTime)
            longestPlayTime = playTime;

        totalPlayTime += playTime;

        // ===== SAVE =====
        PlayerPrefs.SetInt("TotalGames", totalGames);
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetInt("LastScore", score);
        PlayerPrefs.SetInt("TotalScore", totalScore);

        PlayerPrefs.SetFloat("TotalPlayTime", totalPlayTime);
        PlayerPrefs.SetFloat("LongestPlayTime", longestPlayTime);
        PlayerPrefs.SetFloat("LastPlayTime", playTime);

        PlayerPrefs.Save();
    }
}