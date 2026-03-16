using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayButton : MonoBehaviour
{
    public Image image;
    public Sprite normalSprite;
    public Sprite pressedSprite;
    public string sceneName = "MainGame";

    public void OnClick()
    {
        StartCoroutine(PressRoutine());
    }

    IEnumerator PressRoutine()
    {
        image.sprite = pressedSprite;
        yield return new WaitForSeconds(0.1f);
        image.sprite = normalSprite;

        SceneManager.LoadScene(sceneName);
    }
}