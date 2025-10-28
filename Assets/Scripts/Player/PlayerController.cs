using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    // Movement
    public float moveSpeed = 10f;               // Player movement speed
    public float jumpForce = 7f;                // Player jump force
    public float groundCheckRadius = 0.2f;      // Radius for ground check
    private bool isGrounded = false;            // Is the player grounded

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
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, groundLayer);

        // Player Movement Horizontal
        float hValue = Input.GetAxis("Horizontal");

        SpriteFlip(hValue);

        // Set Rigidbody velocity for horizontal movement
        rb.linearVelocityX = hValue * moveSpeed;

        // Player Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Update Animator parameters
        anim.SetFloat("hValue", Mathf.Abs(hValue));
        anim.SetBool("isGrounded", isGrounded);
    }

    private void SpriteFlip (float hValue)
    {
        // Flip sprite based on movement direction - hValue is negative for left, positive for right
        if (hValue != 0)
            sr.flipX = (hValue < 0);
    }
}