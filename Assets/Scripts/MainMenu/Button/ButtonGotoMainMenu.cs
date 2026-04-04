using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenuButton : MonoBehaviour
{
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // tránh bị pause
        SceneManager.LoadScene("MainMenu");
    }
}