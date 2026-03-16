using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuButtonEffect : MonoBehaviour
{
    public Image image;
    public Sprite normalSprite;
    public Sprite pressedSprite;
    public float pressTime = 0.1f;

    public UnityEvent onClick;

    public void OnButtonClick()
    {
        StartCoroutine(PressRoutine());
    }

    IEnumerator PressRoutine()
    {
        image.sprite = pressedSprite;

        yield return new WaitForSeconds(pressTime);

        image.sprite = normalSprite;

        onClick.Invoke();
    }
}