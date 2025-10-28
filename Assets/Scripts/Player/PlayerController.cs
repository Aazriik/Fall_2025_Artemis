using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    // Movement
    public float moveSpeed = 10f;               // Player movement speed
    public float jumpForce = 7f;                // Player jump force
    public float groundCheckRadius = 0.2f;      // Radius for ground check
    private bool isGrounded = false;            // Is the player grounded
    private bool isCrouching = false;           // Is the player crouching
    private bool slamAttack = false;            // Is the player performing a slam attack
    private int jumpCount = 0;                  // Number of jumps made
    private float xVelocitySlowdown = 0;        // Slowdown when crouching
    private float hValue;                       // Horizontal input value
    private bool jumpPressed;                   // Is the jump button pressed

    // Double-press window (makes slam easier to trigger)
    public float doublePressWindow = 1f;
    private float lastJumpPressTime = -10f;


    // Component Refs
    Rigidbody2D rb;                             // Reference to the player's Rigidbody2D
    Collider2D col;                             // Reference to the player's Collider2D
    SpriteRenderer sr;                          // Reference to the player's SpriteRenderer
    Animator anim;                              // Reference to the player's Animator

    // Layer Masks
    private LayerMask groundLayer;              // Layer mask for ground detection

    // Position for ground check
    private Vector2 groundCheckPos => new Vector2(col.bounds.center.x, col.bounds.min.y);

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

        // Set the ground layer mask
        groundLayer = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        // Player Movement Horizontal
        //float hValue = Input.GetAxis("Horizontal");
        // Set Rigidbody velocity for horizontal movement
        //rb.linearVelocityX = hValue * moveSpeed;

        // Player Movement Horizontal
        hValue = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
            lastJumpPressTime = Time.time;
            Debug.Log($"Input: Jump pressed at {lastJumpPressTime}");
        }

        // Player Crouch
        isCrouching = Input.GetButton("Fire1");

        // Flip Sprite
        SpriteFlip(hValue);
    }

    private void FixedUpdate()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle
            (groundCheckPos, groundCheckRadius, groundLayer);

        // Reset when grounded
        if (isGrounded)
        {
            slamAttack = false;
            jumpCount = 0;
            Debug.Log($"Jumped: jumpCount={jumpCount}");
        }

        // Horizontal movement
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(hValue * moveSpeed, rb.linearVelocity.y);
        }

        // Player Jump (first press -> jump; second press in air -> slam)
        if (jumpPressed)
        {
            Debug.Log($"FixedUpdate: jumpPressed true | isGrounded={isGrounded} jumpCount={jumpCount} slamAttack={slamAttack}");

            // First press on ground -> jump
            if (isGrounded && jumpCount == 0)
            {
                if (rb != null)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }
                /*
                // Normal jump
                // reset vertical for consistency
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); */
                jumpCount = 1;
                Debug.Log($"Performed jump. jumpCount={jumpCount}");
            }

            // Second press in air -> slam
            //else if (!isGrounded && jumpCount == 1 && !slamAttack)
            else
            {
                // Two conditions accepted:
                // 1) explicit jumpCount==1 while airborne
                // 2) a double-press within doublePressWindow (helps if player presses quickly)
                bool inAirDoublePress = !isGrounded && jumpCount == 1 && !slamAttack;
                bool timeDoublePress = Time.time - lastJumpPressTime <= doublePressWindow && !isGrounded && !slamAttack;

                if (inAirDoublePress || timeDoublePress)
                {
                    Debug.Log($"Slam conditions met: inAirDoublePress={inAirDoublePress} timeDoublePress={timeDoublePress}");
                    SlamAttack();
                }
                else
                {
                    Debug.Log("SlamAttack conditions not met.");
                }
            }

            jumpPressed = false;
        }

        // Player Crouch
        if (isCrouching && isGrounded)
        {
            // Reduce player speed to zero over 1 second when crouching
            //xVelocitySlowdown += Time.deltaTime;
            //Mathf.Clamp(xVelocitySlowdown, 0, 1f);
            //rb.linearVelocityX = Mathf.Lerp(rb.linearVelocityX, 0, xVelocitySlowdown);
            xVelocitySlowdown += Time.fixedDeltaTime;
            xVelocitySlowdown = Mathf.Clamp01(xVelocitySlowdown);
            float newX = Mathf.Lerp(rb.linearVelocity.x, 0f, xVelocitySlowdown);
            rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
        }
        else
        {
            // Normal movement speed when not crouching
            //rb.linearVelocityX = hValue * moveSpeed;
            xVelocitySlowdown = 0f;
        }

        #region Animator Parameters

        // Update Animator parameters
        if (anim != null)
        {
            anim.SetFloat("hValue", Mathf.Abs(hValue));
            anim.SetBool("isGrounded", isGrounded);
            anim.SetBool("slamAttack", slamAttack);
            anim.SetInteger("jumpCount", jumpCount);
            // Inverted Crouch logic for Animator parameter
            anim.SetBool("isCrouching", !isCrouching);
        }

        #endregion
    }

    private void SpriteFlip(float hValue)
    {
        // Flip sprite based on movement direction - hValue is negative for left, positive for right
        if (hValue != 0)
            sr.flipX = (hValue < 0);
    }

    /*private void PlayerJump()
    {
        // Player Jump
        if (Input.GetButtonDown("Jump") && jumpCount == 0 && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
        }
        else if (Input.GetButtonDown("Jump") && jumpCount == 1 && !isGrounded && !slamAttack)
        {
            SlamAttack();
        }
        if (isGrounded)
        {
            slamAttack = false;
            jumpCount = 0;
        }
    }*/

    private void SlamAttack()
    {
        slamAttack = true;

        if (rb != null)
        {
            // stop vertical movement then apply downward impulse
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.down * jumpForce * 2f, ForceMode2D.Impulse);
        }

        Debug.Log("SlamAttack started");

        /*
        // Stop vertical and horizontal movement and apply downward force
        rb.linearVelocity = new Vector2(rb.linearVelocity.x , 0);
        rb.AddForce(Vector2.down * jumpForce * 2, ForceMode2D.Impulse);

        if (isGrounded)
        {
            slamAttack = false;
            jumpCount = 0;
        }*/
    }

    void OnDrawGizmosSelected()
    {
        if (col != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheckPos, groundCheckRadius);
        }
    }
}