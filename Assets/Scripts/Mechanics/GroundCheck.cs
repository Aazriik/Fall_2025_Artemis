// Libraries
using UnityEngine;

public class GroundCheck
{
    // Variables
    private bool isGrounded;
    public bool IsGrounded => isGrounded;

    private LayerMask groundLayer;
    private Collider2D col;
    private Rigidbody2D rb;
    private float groundCheckRadius = 0.2f;
    private Vector2 groundCheckOffPos => new Vector2(col.bounds.center.x, col.bounds.min.y);

    // Constructor
    public GroundCheck(Collider2D col, LayerMask groundLayer, float groundCheckRadius)
    {
        // Initialize variables
        this.col = col;
        // Assign ground layer and check radius
        this.groundLayer = groundLayer;
        // Set the ground check radius
        this.groundCheckRadius = groundCheckRadius;
        // Get the Rigidbody2D component
        rb = col.GetComponent<Rigidbody2D>();
    }

    // Method to check if the player is grounded
    public bool CheckIsGrounded()
    {
        // Only check for grounding if falling or already grounded
        if (!isGrounded && rb.linearVelocityY < 0 || isGrounded)
            isGrounded = Physics2D.OverlapCircle(groundCheckOffPos, groundCheckRadius, groundLayer);
        // Return the grounded status
        return isGrounded;
    }

    // Method to update the ground check radius
    public void UpdateCheckRadius(float newRadius)
    {
        // Update the ground check radius
        groundCheckRadius = newRadius;
        // Log the update
        Debug.Log("Ground check radius updated to: " + groundCheckRadius);
    }
}
