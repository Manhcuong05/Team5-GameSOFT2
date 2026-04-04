using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PropellerPickup : MonoBehaviour
{
    [Header("Propeller Settings")]
    public float flyDuration = 2f;
    public float flySpeed = 12f;
    public GameObject hatVisualPrefab;

    [Header("Pickup Settings")]
    public float heightTolerance = 0.5f; // độ lệch cho phép

    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return;

        Player player = collision.GetComponent<Player>();
        if (player == null) return;

        // 🔥 không ăn khi đang bay
        if (player.IsPropellerFlying) return;

        // 🔥 check ngang nhau theo trục Y
        float playerY = collision.bounds.center.y;
        float itemY = GetComponent<Collider2D>().bounds.center.y;

        float diff = Mathf.Abs(playerY - itemY);

        if (diff > heightTolerance)
        {
            // quá lệch → không ăn
            return;
        }

        // ✅ hợp lệ → ăn
        isCollected = true;

        player.ActivatePropeller(flyDuration, flySpeed, hatVisualPrefab);

        Destroy(gameObject);
    }
}