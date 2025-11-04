using UnityEngine;

public class GroundCheck
{
    private bool isGrounded;
    public bool IsGrounded => isGrounded;

    private LayerMask groundLayer;
    private Collider2D col;
    private Rigidbody2D rb;
    private float groundCheckRadius = 0.2f;
    private Vector2 groundCheckOffPos => new Vector2(col.bounds.center.x, col.bounds.min.y);

    public GroundCheck(Collider2D col, LayerMask groundLayer, float groundCheckRadius)
    {
        this.col = col;
        this.groundLayer = groundLayer;
        this.groundCheckRadius = groundCheckRadius;
        rb = col.GetComponent<Rigidbody2D>();
    }

    public bool CheckIsGrounded()
    {
        if (!isGrounded && rb.linearVelocityY < 0 || isGrounded)
            isGrounded = Physics2D.OverlapCircle(groundCheckOffPos, groundCheckRadius, groundLayer);
        
        return isGrounded;
    }

    public void UpdateCheckRadius(float newRadius)
    {
        groundCheckRadius = newRadius;
        Debug.Log("Ground check radius updated to: " + groundCheckRadius);
    }
}
