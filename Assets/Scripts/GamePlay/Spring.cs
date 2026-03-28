using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Spring : MonoBehaviour
{
    [Header("Spring Settings")]
    public float springForce = 16f;

    private bool isUsed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isUsed) return;

        Player player = collision.GetComponent<Player>();
        if (player == null) return;

        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // Chỉ kích hoạt khi player đang rơi xuống và ở phía trên spring
        if (rb.velocity.y <= 0f && collision.transform.position.y > transform.position.y)
        {
            isUsed = true;
            player.Bounce(springForce);
            Destroy(gameObject);
        }
    }
}