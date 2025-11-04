using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    #region Variables

    // Movement
    public float moveSpeed = 10f;               // Player movement speed
    public float groundCheckRadius = 0.2f;      // Radius for ground check
    private bool isGrounded = false;            // Is the player grounded


    // Component Refs
    Rigidbody2D rb;                             // Reference to the player's Rigidbody2D
    Collider2D col;                             // Reference to the player's Collider2D
    SpriteRenderer sr;                          // Reference to the player's SpriteRenderer
    Animator anim;                              // Reference to the player's Animator
    GroundCheck groundCheck;                    // Reference to GroundCheck script

    //public float Gravity => -(2 * MaxJumpHeight) / (TimeToJumpApex * TimeToJumpApex);
    //public float JumpVelocity => Mathf.Abs(Gravity) * TimeToJumpApex;

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
            rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
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
}