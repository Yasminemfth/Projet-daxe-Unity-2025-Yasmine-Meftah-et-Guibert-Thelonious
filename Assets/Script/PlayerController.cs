using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Rigidbody2D rb;
    public Collider2D playerCollider;

    [Header("Stats")]
    [SerializeField] private PlayerStats _stat;

    [Header("Mouvement & Saut")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jetpackForce = 8f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float wallJumpForce = 16f;
    [SerializeField] private float wallJumpHorizontalForce = 8f;
    [SerializeField] private float wallSlideSpeed = 2f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float normalizeRotationSpeed = 3f;
    [SerializeField] private float maxTiltAngle = 20f;

    [Header("Jetpack")]
    [SerializeField] private float fuel = 100f;
    [SerializeField] private float fuelBurnRate = 18f;
    [SerializeField] private float fuelRefillRate = 20f;
    [SerializeField] private Slider fuelSlider;

    [Header("Sol & Murs")]
    [SerializeField] private float boxLength = 1f;
    [SerializeField] private float boxHeight = 0.2f;
    [SerializeField] private float wallCheckDistance = 0.6f;
    [SerializeField] private Transform groundPosition;
    [SerializeField] private Transform wallCheckPosition;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private float moveInput;
    private bool isFlying;
    private bool jumpPressed;
    private bool jumpBuffered;
    private float jumpBufferTimer;
    private float jumpBufferTime = 0.15f;
    private float coyoteTimer;
    private float coyoteTime = 0.15f;

    private bool canDoubleJump;
    private bool grounded;
    private bool touchingWall;
    private bool isWallSliding;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;

    private float currentFuel;
    private bool isFacingRight = true;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.gravityScale = 0f; // desactive la gravité

        currentFuel = fuel;

        //  stats du struct
        if (_stat.speed > 0) moveSpeed = _stat.speed;
        if (_stat.life > 0) jumpForce = _stat.life;
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isFlying = Input.GetKey(KeyCode.E) && currentFuel > 0f;
        jumpPressed = Input.GetButtonDown("Jump");
        fuelSlider.value = currentFuel / fuel;

        if (jumpPressed)
        {
            jumpBuffered = true;
            jumpBufferTimer = jumpBufferTime;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
            if (jumpBufferTimer <= 0f) jumpBuffered = false;
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapBox(groundPosition.position, new Vector2(boxLength, boxHeight), 0f, groundLayer);
        coyoteTimer = grounded ? coyoteTime : coyoteTimer - Time.fixedDeltaTime;

        touchingWall = Physics2D.Raycast(transform.position, Vector2.right * (isFacingRight ? 1 : -1), wallCheckDistance, wallLayer);

        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        HandleJump();

        if (isFlying)
        {
            rb.AddForce(Vector2.up * jetpackForce, ForceMode2D.Force);
            currentFuel -= fuelBurnRate * Time.fixedDeltaTime;
        }

        if (grounded) RefillFuel();

        float targetAngle = Mathf.Clamp(-moveInput * maxTiltAngle, -maxTiltAngle, maxTiltAngle);
        float smoothAngle = Mathf.LerpAngle(rb.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(smoothAngle);

        if (grounded && Mathf.Approximately(moveInput, 0f))
        {
            float uprightAngle = Mathf.LerpAngle(rb.rotation, 0f, normalizeRotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(uprightAngle);
        }

        currentFuel = Mathf.Clamp(currentFuel, 0f, fuel);
    }

    void HandleJump()
    {
        if (jumpBuffered && coyoteTimer > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            canDoubleJump = true;
            isWallJumping = false;
            jumpBuffered = false;
        }
        else if (jumpBuffered && canDoubleJump && !grounded && !touchingWall)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            canDoubleJump = false;
            jumpBuffered = false;
        }
    }

    void WallSlide()
    {
        isWallSliding = touchingWall && !grounded && moveInput != 0;
        if (isWallSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        }
    }

    void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else wallJumpingCounter -= Time.deltaTime;

        if (jumpPressed && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpingDirection * wallJumpHorizontalForce, wallJumpForce);
            wallJumpingCounter = 0f;
            canDoubleJump = true;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    void StopWallJumping() => isWallJumping = false;

    void Flip()
    {
        if ((isFacingRight && moveInput < 0f) || (!isFacingRight && moveInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void RefillFuel()
    {
        if (currentFuel < fuel)
        {
            currentFuel += fuelRefillRate * Time.fixedDeltaTime;
        }
    }
}
