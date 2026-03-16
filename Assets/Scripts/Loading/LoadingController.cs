using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    public string nextScene = "MainMenu";
    public float delay = 2f;

    void Start()
    {
        Invoke(nameof(GoNext), delay);
    }

    void GoNext()
    {
        SceneManager.LoadScene(nextScene);
    }
}
