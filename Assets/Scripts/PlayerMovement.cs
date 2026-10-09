using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private Vector3 respawnPosition;
    public Animator animator;
    bool isFacingRight = true;
    public ParticleSystem smokeFX;
    public DeathCounterUI deathCounter;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;

    [Header("Jumping")]
    public float jumpPower = 8f;
    int jumpsRemaining;

    [Header("WallMovement")]
    public float wallSlideSpeed = 2;
    bool isWallSliding;

    [Header("Abilities")]
    public bool hasDoubleJump = false;
    public bool hasWallJump = false;

    [Header("GroundCheck")]
    public Transform groundCheckPosition;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool isGrounded;
    bool wasGrounded;

    [Header("WallCheck")]
    public Transform wallCheckPosition;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;

    [Header("WallJumping")]
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);

    void Start()
    {
        respawnPosition = transform.position;

        hasDoubleJump = PlayerPrefs.GetInt("HasDoubleJump", 0) == 1;
        hasWallJump = PlayerPrefs.GetInt("HasWallJump", 0) == 1;

        jumpsRemaining = GetMaxJumps();
    }

    void Update()
    {
        ProcessGravity();
        GroundCheck();
        ProcessWallSlide();
        ProcessWallJump();
    }

    void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
            Flip();
        }

        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetFloat("magnitude", rb.linearVelocity.magnitude);
        animator.SetBool("isWallSliding", isWallSliding);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Hazard"))
        {
            Die();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && jumpsRemaining > 0)
        {
            bool jumpedFromGround = isGrounded;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            jumpsRemaining--;
            animator.SetTrigger("jump");

            SFXManager.Instance.Play(
                SFXManager.Instance.jumpSound
            );

            if (jumpedFromGround)
            {
                smokeFX.Play();
            }
        }

        // short jump
        if (context.canceled && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // wall jump
        if (context.performed && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpTimer = 0;
            animator.SetTrigger("jump");
            smokeFX.Play();

            SFXManager.Instance.Play(
                SFXManager.Instance.jumpSound
            );

            // force flip
            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }

            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
        }
    }

    private void Die()
    {
        SFXManager.Instance.Play(
            SFXManager.Instance.deathSound
        );

        deathCounter.AddDeath();

        transform.position = respawnPosition;
        rb.linearVelocity = Vector2.zero;
    }

    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        respawnPosition = newCheckpoint;
    }

    private void ProcessGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void GroundCheck()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapBox(groundCheckPosition.position, groundCheckSize, 0, groundLayer);

        // has just landed
        if (isGrounded && !wasGrounded)
        {
            jumpsRemaining = GetMaxJumps();
        }
    }

    private int GetMaxJumps()
    {
        return hasDoubleJump ? 2 : 1;
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPosition.position, wallCheckSize, 0, wallLayer);
    }

    private void ProcessWallSlide()
    {
        if (!hasWallJump)
        {
            isWallSliding = false;
            return;
        }

        if (!isGrounded && WallCheck() && horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void ProcessWallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;

            if (rb.linearVelocity.y == 0)
            {
                smokeFX.Play();
            }
        }
    }

    public void UnlockDoubleJump()
    {
        hasDoubleJump = true;

        PlayerPrefs.SetInt("HasDoubleJump", 1);
        PlayerPrefs.Save();

        if (isGrounded)
        {
            jumpsRemaining = GetMaxJumps();
        }
    }

    public void UnlockWallJump()
    {
        hasWallJump = true;

        PlayerPrefs.SetInt("HasWallJump", 1);
        PlayerPrefs.Save();
    }

    private void OnDrawGizmosSelected()
    {
        // ground
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPosition.position, groundCheckSize);

        // wall
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPosition.position, wallCheckSize);
    }
}
