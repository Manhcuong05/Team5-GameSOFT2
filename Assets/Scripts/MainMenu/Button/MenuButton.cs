using System.Collections;
using UnityEngine;

public abstract class MenuUIButtonBase : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite pressedSprite;

    public int flashCount = 1;
    public float flashDuration = 0.1f;

    SpriteRenderer sr;
    bool isRunning;

    protected virtual void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        if (normalSprite != null)
            sr.sprite = normalSprite;
    }

    void OnMouseDown()
    {
        if (!isRunning)
            StartCoroutine(FlashThenAction());
    }

    IEnumerator FlashThenAction()
    {
        isRunning = true;

        for (int i = 0; i < flashCount; i++)
        {
            sr.sprite = pressedSprite;
            yield return new WaitForSeconds(flashDuration);

            sr.sprite = normalSprite;
            yield return new WaitForSeconds(flashDuration);
        }

        sr.sprite = normalSprite;

        yield return OnAfterFlash();

        isRunning = false;
    }

    protected abstract IEnumerator OnAfterFlash();
}