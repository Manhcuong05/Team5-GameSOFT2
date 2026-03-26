using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float movementSpeed = 10f;

    private Rigidbody2D rb;
    private float movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // 👉 Giả lập nghiêng điện thoại bằng A/D
        float fakeTilt = 0f;

        if (Input.GetKey(KeyCode.A))
            fakeTilt = -1f;

        if (Input.GetKey(KeyCode.D))
            fakeTilt = 1f;

        movement = fakeTilt;
#else
        // 👉 Mobile thật (nghiêng điện thoại)
        movement = Input.acceleration.x;
#endif
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movement * movementSpeed, rb.velocity.y);
    }
}