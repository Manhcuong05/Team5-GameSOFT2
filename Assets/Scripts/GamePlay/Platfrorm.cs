using UnityEngine;

public class Platfrorm : MonoBehaviour
{
    public float jumpForce = 10f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y <= 0f)
        {
            Player player = collision.collider.GetComponent<Player>();
            if (player != null)
            {
                player.Bounce(jumpForce);
            }
        }
    }
}