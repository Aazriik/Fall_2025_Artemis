// Libraries
//using System.Collections;
using UnityEngine;

// Required Components
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    #region Control Variables
    // Player Health
    [Header("Health Settings")]
    public int maxLives = 10;
    private int _lives = 5;

    // Movement
    [Header("Movement Settings")]
    public float moveSpeed = 10f;               // Player movement speed

    [Header("Jump Settings")]
    public float initalPowerUpTimer = 5f;       // Initial duration of jump power-up
    public float jumpForce = 10f;               // Force applied when jumping
    public float groundCheckRadius = 0.2f;      // Radius for ground check
    private bool isGrounded = false;            // Is the player grounded

    //public float Gravity => -(2 * MaxJumpHeight) / (TimeToJumpApex * TimeToJumpApex);
    //public float JumpVelocity => Mathf.Abs(Gravity) * TimeToJumpApex;

    #endregion

    #region Component References
    // Component Refs
    private Rigidbody2D rb;                     // Reference to the player's Rigidbody2D
    private Collider2D col;                     // Reference to the player's Collider2D
    private SpriteRenderer sr;                  // Reference to the player's SpriteRenderer
    private Animator anim;                      // Reference to the player's Animator
    private GroundCheck groundCheck;            // Reference to GroundCheck script

    #endregion

    #region State Variables
    // State Variables
    private Coroutine jumpForceCoroutine = null;
    private float jumpPowerupTimer = 0f;        // Duration of jump power-up

    #endregion

    // Start is called once at creation
    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        // Get the Collider2D component
        col = GetComponent<Collider2D>();
        // Get the SpriteRenderer component
        sr = GetComponent<SpriteRenderer>();
        // Get the Animator component
        anim = GetComponent<Animator>();
        // Initialize GroundCheck
        groundCheck = new GroundCheck(col, LayerMask.GetMask("Ground"), groundCheckRadius);

    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = groundCheck.CheckIsGrounded();

        // Player Movement Horizontal
        float hValue = Input.GetAxis("Horizontal");
        float vValue = Input.GetAxisRaw("Vertical");
        // Flip Sprite
        SpriteFlip(hValue);

        rb.linearVelocityX = hValue * moveSpeed;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //apply an upward force to the rigidbody when the jump button is pressed
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Fire");
        }

        //update animator parameters
        anim.SetFloat("hValue", Mathf.Abs(hValue));
        anim.SetBool("isGrounded", isGrounded);
}

    private void OnValidate() => groundCheck?.UpdateCheckRadius(groundCheckRadius);

    private void SpriteFlip(float hValue)
    {
        // Flip sprite based on movement direction
        // hValue is negative for left, positive for right
        if (hValue != 0)
            sr.flipX = (hValue < 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Squish") && rb.linearVelocityY < 0)
        {
            collision.GetComponentInParent<BaseEnemy>().TakeDamage(0, DamageType.JumpedOn);
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    // Method to apply jump force power-up
    public void ApplyJumpForcePowerup()
    {
        // If a jump force coroutine is already running, stop it
        if (jumpForceCoroutine != null)
        {
            // Stop the existing coroutine
            StopCoroutine(jumpForceCoroutine);
            // Reset jump force to default
            jumpForceCoroutine = null;
            // Reset jump force to default
            jumpForce = 7f;
        }
        // Start a new jump force coroutine
        jumpForceCoroutine = StartCoroutine(JumpForceCoroutine());
    }

    // Coroutine to handle jump force power-up duration
    System.Collections.IEnumerator JumpForceCoroutine()
    {
        jumpPowerupTimer = initalPowerUpTimer + jumpPowerupTimer;
        jumpForce = 10;

        while (jumpPowerupTimer > 0)
        {
            jumpPowerupTimer -= Time.deltaTime;
            Debug.Log("Jump Powerup Timer: " + jumpPowerupTimer);
            yield return null;
        }

        jumpForce = 7;
        jumpForceCoroutine = null;
        jumpPowerupTimer = 0;
    }

    public int lives
    {
        get => _lives;
        set
        {
            if (value < 0)
            {
                GameOver();
                return;
            }

            if (value > maxLives)
            {
                _lives = maxLives;
            }
            else
            {
                _lives = value;
            }

            Debug.Log($"Life value has changed to {_lives}");
        }
    }

    private void GameOver()
    {
        Debug.Log("GameOver!");
    }
}