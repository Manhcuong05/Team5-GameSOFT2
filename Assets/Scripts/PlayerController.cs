using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpVelocity = 12f;
    public float screenWrapPadding = 0.2f;

    Rigidbody2D rb;
    Camera cam;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);

        ScreenWrap();
    }

    void ScreenWrap()
    {
        Vector3 pos = transform.position;
        Vector3 view = cam.WorldToViewportPoint(pos);

        if (view.x < -screenWrapPadding) view.x = 1 + screenWrapPadding;
        else if (view.x > 1 + screenWrapPadding) view.x = -screenWrapPadding;

        transform.position = cam.ViewportToWorldPoint(new Vector3(view.x, view.y, cam.nearClipPlane));
        transform.position = new Vector3(transform.position.x, pos.y, pos.z);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.collider.CompareTag("Platform")) return;
        if (rb.velocity.y <= 0f)
        {
            ContactPoint2D contact = col.contacts[0];
            if (contact.normal.y > 0.5f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
            }
        }
    }
}
