using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;
    private float movement;

    private bool isPropellerFlying = false;
    private Coroutine propellerCoroutine;
    private GameObject currentHatVisual;
    private float originalGravityScale;

    private bool hasStarted = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
    }

    void Start()
    {
        SetupStartPosition();
        StartCoroutine(StartRoutine());
    }

    IEnumerator StartRoutine()
    {
        yield return new WaitForSeconds(startDelay);
        StartGameJump();
    }

    void SetupStartPosition()
    {
        if (Camera.main != null)
        {
            float startY = Camera.main.transform.position.y - 6f;
            transform.position = new Vector3(0f, startY, 0f);
        }
    }

    public void StartGameJump()
    {
        rb.velocity = new Vector2(0f, startJumpForce);
        hasStarted = true;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            Debug.Log("A");

        if (Input.GetKey(KeyCode.D))
            Debug.Log("D");
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movement * speed, rb.velocity.y);
    }
}