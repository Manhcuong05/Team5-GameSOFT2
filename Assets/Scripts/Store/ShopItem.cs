using UnityEngine;
using System.Collections;

public class ShopItem : MonoBehaviour
{
    public float jumpHeight = 1.5f;
    public float jumpDuration = 0.3f;

    private Vector3 startPos;
    private Coroutine jumpLoop;

    void Start()
    {
        startPos = transform.position;
    }

    void OnMouseDown()
    {
        ShopManager.Instance.SelectItem(this);
    }

    // 👉 bắt đầu nhảy liên tục
    public void StartJumpLoop()
    {
        if (jumpLoop == null)
        {
            jumpLoop = StartCoroutine(JumpLoop());
        }
    }

    // 👉 dừng nhảy
    public void StopJumpLoop()
    {
        if (jumpLoop != null)
        {
            StopCoroutine(jumpLoop);
            jumpLoop = null;
        }

        transform.position = startPos; // reset lại vị trí
    }

    IEnumerator JumpLoop()
    {
        while (true)
        {
            float time = 0;

            while (time < jumpDuration)
            {
                float t = time / jumpDuration;
                float y = Mathf.Sin(t * Mathf.PI) * jumpHeight;

                transform.position = startPos + Vector3.up * y;

                time += Time.deltaTime;
                yield return null;
            }

            transform.position = startPos;
            yield return null;
        }
    }
}