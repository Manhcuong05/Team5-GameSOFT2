using System.Collections.Generic;
using UnityEngine;

public class AchievementTracker : MonoBehaviour
{
    public static AchievementTracker Instance;

    [System.Serializable]
    public class AchievementData
    {
        public string id;
        public string title;
        public bool unlocked;

        public AchievementData(string id, string title)
        {
            this.id = id;
            this.title = title;
            this.unlocked = false;
        }
    }

    [Header("Runtime")]
    public Transform player;
    public bool runStarted;
    public bool runEnded;

    public int currentScore;
    public int jumpCount;
    public int consecutiveJumps;
    public float maxHeight;
    public float survivalTime;

    [Header("Achievement List")]
    public List<AchievementData> achievements = new List<AchievementData>();

    private Dictionary<string, AchievementData> achievementMap = new Dictionary<string, AchievementData>();
    private float startY;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitAchievements();
    }

    void Update()
    {
        if (!runStarted || runEnded) return;

        survivalTime += Time.deltaTime;

        if (player != null)
        {
            float currentHeight = player.position.y - startY;
            if (currentHeight > maxHeight)
                maxHeight = currentHeight;
        }

        // Lấy score thật từ GameManager thay vì tự cộng theo bounce
        if (GameManager.Instance != null)
        {
            currentScore = GameManager.Instance.GetScore();
        }

        CheckAchievements();
    }

    void InitAchievements()
    {
        achievements.Clear();
        achievementMap.Clear();

        AddAchievement("jump_3", "Jump 3");
        AddAchievement("jump_10", "Jump 10");
        AddAchievement("score_100", "Score 100");
        AddAchievement("score_200", "Score 200");
        AddAchievement("combo_5", "Combo 5");
        AddAchievement("height_100", "Height 100");
        AddAchievement("height_250", "Height 250");
        AddAchievement("survive_30", "Survive 30");

        foreach (var a in achievements)
        {
            a.unlocked = PlayerPrefs.GetInt("achievement_" + a.id, 0) == 1;
            achievementMap[a.id] = a;
        }
    }

    void AddAchievement(string id, string title)
    {
        achievements.Add(new AchievementData(id, title));
    }

    public void StartNewRun(Transform playerTransform)
    {
        player = playerTransform;
        runStarted = true;
        runEnded = false;

        currentScore = 0;
        jumpCount = 0;
        consecutiveJumps = 0;
        maxHeight = 0f;
        survivalTime = 0f;

        if (player != null)
            startY = player.position.y;

        Debug.Log("AchievementTracker: New run started");
    }

    // Chỉ đếm bounce, KHÔNG cộng score ở đây nữa
    public void RegisterBounce()
    {
        if (!runStarted || runEnded) return;

        jumpCount++;
        consecutiveJumps++;

        Debug.Log("AchievementTracker Bounce | jumps=" + jumpCount);
        CheckAchievements();
    }

    public void BreakCombo()
    {
        consecutiveJumps = 0;
    }

    public void EndRun()
    {
        if (!runStarted) return;

        // Chốt score thật lần cuối
        if (GameManager.Instance != null)
        {
            currentScore = GameManager.Instance.GetScore();
        }

        runEnded = true;
        CheckAchievements();

        Debug.Log("AchievementTracker: Run ended | final score=" + currentScore);
    }

    void CheckAchievements()
    {
        if (jumpCount >= 3) Unlock("jump_3");
        if (jumpCount >= 10) Unlock("jump_10");

        if (currentScore >= 100) Unlock("score_100");
        if (currentScore >= 200) Unlock("score_200");

        if (consecutiveJumps >= 5) Unlock("combo_5");

        if (maxHeight >= 100f) Unlock("height_100");
        if (maxHeight >= 250f) Unlock("height_250");

        if (survivalTime >= 30f) Unlock("survive_30");
    }

    void Unlock(string id)
    {
        if (!achievementMap.ContainsKey(id)) return;

        AchievementData a = achievementMap[id];
        if (a.unlocked) return;

        a.unlocked = true;
        PlayerPrefs.SetInt("achievement_" + a.id, 1);
        PlayerPrefs.Save();

        Debug.Log("Achievement unlocked: " + a.title);
    }

    public List<AchievementData> GetAchievements()
    {
        return achievements;
    }

    public int GetUnlockedCount()
    {
        int count = 0;
        foreach (var a in achievements)
        {
            if (a.unlocked) count++;
        }
        return count;
    }

    public void ResetAllAchievements()
    {
        foreach (var achievement in achievements)
        {
            achievement.unlocked = false;
            PlayerPrefs.DeleteKey("achievement_" + achievement.id);
        }

        currentScore = 0;
        jumpCount = 0;
        consecutiveJumps = 0;
        maxHeight = 0f;
        survivalTime = 0f;
        runStarted = false;
        runEnded = false;
        player = null;

        PlayerPrefs.Save();

        Debug.Log("AchievementTracker: All achievements reset");
    }
}