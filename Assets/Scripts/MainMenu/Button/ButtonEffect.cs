using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MenuButtonEffect : MonoBehaviour, IPointerClickHandler
{
    public Image image;
    public Sprite normalSprite;
    public Sprite pressedSprite;
    public float pressTime = 0.1f;

    public UnityEvent onClick;

    private bool isProcessing = false;

    void Start()
    {
        Debug.Log("Button READY: " + gameObject.name);
    }

    // 🔥 BẮT CLICK TRỰC TIẾP
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("CLICK DETECTED: " + gameObject.name);

        OnButtonClick();
    }

    public void OnButtonClick()
    {
        Debug.Log("OnButtonClick CALLED");

        if (!isProcessing)
        {
            StartCoroutine(PressRoutine());
        }
    }

    IEnumerator PressRoutine()
    {
        isProcessing = true;

        Debug.Log("Start Press Effect");

        image.sprite = pressedSprite;

        yield return new WaitForSecondsRealtime(pressTime);

        image.sprite = normalSprite;

        Debug.Log("Invoke Event");

        onClick?.Invoke();

        isProcessing = false;
    }
}