using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionMenu;
    public GameObject storeMenu;
    public GameObject scoreMenu;

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
        storeMenu.SetActive(true);
        scoreMenu.SetActive(true);
    }

    public void ShowOptionMenu()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
        storeMenu.SetActive(false);
        scoreMenu.SetActive(false);
    }

    public void ShowStoreMenu()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(false);
        storeMenu.SetActive(true);
        scoreMenu.SetActive(false);
    }

    public void ShowScoreMenu()
    {
        SceneManager.LoadScene("ScoresScene");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}