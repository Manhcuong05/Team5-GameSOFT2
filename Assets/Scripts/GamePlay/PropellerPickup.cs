using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PropellerPickup : MonoBehaviour
{
    [Header("Propeller Settings")]
    public float flyDuration = 2f;
    public float flySpeed = 12f;
    public GameObject hatVisualPrefab;

    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return;

        Player player = collision.GetComponent<Player>();
        if (player == null) return;

        isCollected = true;

        player.ActivatePropeller(flyDuration, flySpeed, hatVisualPrefab);

        Destroy(gameObject);
    }
}