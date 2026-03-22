using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
public static ScoreManager instance;

public List<int> scores = new List<int>();

void Awake()
{
    if (instance == null)
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

public void AddScore(int score)
{
    scores.Add(score);

    scores.Sort();
    scores.Reverse();

    if (scores.Count > 10)
    {
        scores.RemoveAt(scores.Count - 1);
    }
}

// =========================
// BUTTON MENU
// =========================

public void GoToMainMenu()
{
    SceneManager.LoadScene("MainMenu");
}
}
