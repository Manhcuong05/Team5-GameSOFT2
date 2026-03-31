using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed = 10f;
    public float jumpForce = 10f;

    [Header("Start Jump")]
    public float startJumpForce = 20f;
    public float startDelay = 0.3f;

    [Header("Wrap Screen")]
    public float wrapOffset = 0.1f;

    [Header("Propeller")]
    public Transform hatAnchor;
    public float propellerFlySpeed = 12f;

    private Rigidbody2D rb;
    private float movement;

    private bool isPropellerFlying = false;
    private Coroutine propellerCoroutine;
    private GameObject currentHatVisual;
    private float originalGravityScale;

    private bool hasStarted = false;
    private bool isGameOver = false;

    // 👉 expose cho Pickup dùng
    public bool IsPropellerFlying => isPropellerFlying;

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
#if UNITY_EDITOR || UNITY_STANDALONE
        float fakeTilt = 0f;

        if (Input.GetKey(KeyCode.A))
            fakeTilt = -1f;

        if (Input.GetKey(KeyCode.D))
            fakeTilt = 1f;

        movement = fakeTilt;
#else
        movement = Input.acceleration.x;
#endif

        WrapScreen();
        CheckGameOver(); // ✅ FIX: gọi hàm này
    }

    void FixedUpdate()
    {
        float verticalVelocity = rb.velocity.y;

        if (isPropellerFlying)
        {
            verticalVelocity = propellerFlySpeed;
        }

        float horizontal = hasStarted ? movement : 0f;

        rb.velocity = new Vector2(horizontal * movementSpeed, verticalVelocity);
    }

    public void Bounce()
    {
        if (isPropellerFlying) return;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        // 👉 cộng điểm khi nhảy (tuỳ bạn chỉnh)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(10);
        }
    }

    public void Bounce(float force)
    {
        if (isPropellerFlying) return;
        rb.velocity = new Vector2(rb.velocity.x, force);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(10);
        }
    }

    // 🔥 không cho stack propeller
    public void ActivatePropeller(float duration, float flySpeed, GameObject hatVisualPrefab)
    {
        if (isPropellerFlying) return;

        propellerCoroutine = StartCoroutine(PropellerRoutine(duration, flySpeed, hatVisualPrefab));
    }

    private IEnumerator PropellerRoutine(float duration, float flySpeed, GameObject hatVisualPrefab)
    {
        isPropellerFlying = true;
        propellerFlySpeed = flySpeed;

        rb.velocity = new Vector2(rb.velocity.x, propellerFlySpeed);
        rb.gravityScale = 0f;

        SpawnHatVisual(hatVisualPrefab);

        yield return new WaitForSeconds(duration);

        isPropellerFlying = false;
        rb.gravityScale = originalGravityScale;

        ClearCurrentHatVisual();
        propellerCoroutine = null;
    }

    private void SpawnHatVisual(GameObject hatVisualPrefab)
    {
        if (hatVisualPrefab == null || hatAnchor == null) return;

        currentHatVisual = Instantiate(hatVisualPrefab, hatAnchor);
        currentHatVisual.transform.localPosition = Vector3.zero;
        currentHatVisual.transform.localRotation = Quaternion.identity;
        currentHatVisual.transform.localScale = Vector3.one;

        SpriteRenderer hatSR = currentHatVisual.GetComponent<SpriteRenderer>();
        SpriteRenderer playerSR = GetComponent<SpriteRenderer>();

        if (hatSR != null && playerSR != null)
        {
            hatSR.sortingLayerID = playerSR.sortingLayerID;
            hatSR.sortingOrder = playerSR.sortingOrder + 1;
        }
    }

    private void ClearCurrentHatVisual()
    {
        if (currentHatVisual != null)
        {
            Destroy(currentHatVisual);
            currentHatVisual = null;
        }
    }

    private void WrapScreen()
    {
        if (Camera.main == null) return;

        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 worldPos;

        if (viewPos.x < -wrapOffset)
        {
            viewPos.x = 1f + wrapOffset;
            worldPos = Camera.main.ViewportToWorldPoint(viewPos);
            transform.position = new Vector3(worldPos.x, transform.position.y, transform.position.z);
        }
        else if (viewPos.x > 1f + wrapOffset)
        {
            viewPos.x = -wrapOffset;
            worldPos = Camera.main.ViewportToWorldPoint(viewPos);
            transform.position = new Vector3(worldPos.x, transform.position.y, transform.position.z);
        }
    }

    // ✅ FIX: thêm lại hàm này
    void CheckGameOver()
    {
        if (!hasStarted || Camera.main == null || isGameOver) return;

        float cameraY = Camera.main.transform.position.y;

        if (transform.position.y < cameraY - 6f)
        {
            isGameOver = true; // ⭐ CHẶN GỌI LẠI

            if (GameManager.Instance != null)
            {
                int score = GameManager.Instance.GetScore();

                if (GameStatsTracker.instance != null)
                {
                    GameStatsTracker.instance.EndGame(score);
                }

                GameManager.Instance.GameOver();
            }
        }
    }
}