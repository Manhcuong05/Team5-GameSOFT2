using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Spring : MonoBehaviour
{
    [Header("Spring Settings")]
    public float springForce = 16f;

    [Header("Sprites")]
    public Sprite idleSprite;   // chưa chạm
    public Sprite usedSprite;   // đã chạm

    private bool isUsed = false;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = idleSprite; // set mặc định
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isUsed) return;

        Player player = collision.GetComponent<Player>();
        if (player == null) return;

        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        if (rb.velocity.y <= 0f && collision.transform.position.y > transform.position.y)
        {
            isUsed = true;

            // 👉 đổi sprite khi kích hoạt
            sr.sprite = usedSprite;

            player.Bounce(springForce);

            // 👉 delay chút rồi destroy (để thấy animation)
            Destroy(gameObject, 0.2f);
        }
    }
}