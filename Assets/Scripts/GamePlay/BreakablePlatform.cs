using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class BreakPlatform : MonoBehaviour
{
    [Header("Break Timing")]
    public float breakDelay = 0.05f;   // delay cho player kịp bounce
    public float breakDuration = 0.2f;

    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite brokenSprite;

    [Header("Fake Break Visual")]
    public float shrinkMultiplier = 0.7f;
    public float rotateAngle = 20f;
    public float fallDistance = 0.5f;

    private Collider2D col;
    private SpriteRenderer sr;
    private bool isBroken = false;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        if (normalSprite != null)
            sr.sprite = normalSprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryBreak(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryBreak(collision);
    }

    private void TryBreak(Collision2D collision)
    {
        if (isBroken) return;

        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null) return;

        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // 🔥 chỉ khi đang rơi xuống
        if (rb.velocity.y > 0f) return;

        // 🔥 phải ở trên platform
        if (player.transform.position.y < transform.position.y) return;

        // ✅ bounce trước
        player.Bounce();

        StartCoroutine(BreakSequence());
    }

    private IEnumerator BreakSequence()
    {
        if (isBroken) yield break;
        isBroken = true;

        // 🔥 đổi sprite sang broken
        if (brokenSprite != null)
            sr.sprite = brokenSprite;

        // 👉 delay để player kịp nhảy
        yield return new WaitForSeconds(breakDelay);

        if (col != null)
            col.enabled = false;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.down * fallDistance;

        Vector3 startScale = transform.localScale;
        Vector3 endScale = startScale * shrinkMultiplier;

        float randomAngle = Random.Range(-rotateAngle, rotateAngle);
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(0f, 0f, randomAngle);

        Color startColor = sr.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        float elapsed = 0f;

        while (elapsed < breakDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / breakDuration;

            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);
            sr.color = Color.Lerp(startColor, endColor, t);

            yield return null;
        }

        Destroy(gameObject);
    }
}