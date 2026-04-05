using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Spring : MonoBehaviour
{
    [Header("Spring Settings")]
    public float springForce = 16f;

    [Header("Sprites")]
    public Sprite idleSprite;
    public Sprite usedSprite;

    [Header("Sound")]
    public AudioClip springSound;

    [Range(0f, 3f)]
    public float soundVolume = 1.5f;

    private bool isUsed = false;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = idleSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isUsed) return;

        Player player = collision.GetComponent<Player>();
        if (player == null) return;

        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        bool hitFromAbove = rb.velocity.y <= 0f &&
                            collision.transform.position.y > transform.position.y;

        if (!hitFromAbove) return;

        isUsed = true;
        sr.sprite = usedSprite;

        if (springSound != null)
        {
            AudioSource.PlayClipAtPoint(
                springSound,
                Camera.main.transform.position,
                soundVolume
            );
        }

        player.Bounce(springForce, false);

        Destroy(gameObject, 0.2f);
    }
}