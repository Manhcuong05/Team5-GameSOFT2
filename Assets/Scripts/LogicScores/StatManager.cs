using UnityEngine;

public class GameStatsTracker : MonoBehaviour
{
    private float playTime = 0f;
    private bool isPlaying = false;
    public static GameStatsTracker instance;

    private const string TotalGamesKey = "TotalGames";
    private const string HighScoreKey = "HighScore";
    private const string LastScoreKey = "LastScore";
    private const string TotalScoreKey = "TotalScore";
    private const string TotalPlayTimeKey = "TotalPlayTime";
    private const string LongestPlayTimeKey = "LongestPlayTime";
    private const string LastPlayTimeKey = "LastPlayTime";

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

        int totalGames = PlayerPrefs.GetInt(TotalGamesKey, 0);
        int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        int totalScore = PlayerPrefs.GetInt(TotalScoreKey, 0);

        float totalPlayTime = PlayerPrefs.GetFloat(TotalPlayTimeKey, 0);
        float longestPlayTime = PlayerPrefs.GetFloat(LongestPlayTimeKey, 0);

        totalGames++;
        totalScore += score;

        if (score > highScore)
            highScore = score;

        if (playTime > longestPlayTime)
            longestPlayTime = playTime;

        totalPlayTime += playTime;

        PlayerPrefs.SetInt(TotalGamesKey, totalGames);
        PlayerPrefs.SetInt(HighScoreKey, highScore);
        PlayerPrefs.SetInt(LastScoreKey, score);
        PlayerPrefs.SetInt(TotalScoreKey, totalScore);

        PlayerPrefs.SetFloat(TotalPlayTimeKey, totalPlayTime);
        PlayerPrefs.SetFloat(LongestPlayTimeKey, longestPlayTime);
        PlayerPrefs.SetFloat(LastPlayTimeKey, playTime);

        PlayerPrefs.Save();
    }

    public void ResetStats()
    {
        isPlaying = false;
        playTime = 0f;

        PlayerPrefs.DeleteKey(TotalGamesKey);
        PlayerPrefs.DeleteKey(HighScoreKey);
        PlayerPrefs.DeleteKey(LastScoreKey);
        PlayerPrefs.DeleteKey(TotalScoreKey);
        PlayerPrefs.DeleteKey(TotalPlayTimeKey);
        PlayerPrefs.DeleteKey(LongestPlayTimeKey);
        PlayerPrefs.DeleteKey(LastPlayTimeKey);

        PlayerPrefs.Save();
    }
}