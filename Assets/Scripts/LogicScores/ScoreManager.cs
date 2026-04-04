using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [HideInInspector]
    public List<int> scores = new List<int>();

    private const string SAVE_KEY = "SCORE_DATA";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            scores.Clear();
            LoadScores();

            Debug.Log("ScoreManager Loaded");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =========================
    // ADD SCORE
    // =========================
    public void AddScore(int score)
    {
        scores.Add(score);

        scores.Sort();
        scores.Reverse();

        if (scores.Count > 10)
        {
            scores.RemoveAt(scores.Count - 1);
        }

        SaveScores();
    }

    public List<int> GetScores()
    {
        return scores;
    }

    // =========================
    // SAVE / LOAD
    // =========================
    void SaveScores()
    {
        string json = JsonUtility.ToJson(new Wrapper { list = scores });
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    void LoadScores()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            Wrapper data = JsonUtility.FromJson<Wrapper>(json);

            if (data != null && data.list != null)
            {
                scores = data.list;
            }
        }
    }

    // =========================
    // RESET (GỌI TỪ BUTTON)
    // =========================
    public void ResetScores()
    {
        scores.Clear();
        PlayerPrefs.DeleteKey(SAVE_KEY);
        PlayerPrefs.Save();

        Debug.Log("Scores reset!");
    }

    [System.Serializable]
    class Wrapper
    {
        public List<int> list;
    }
}