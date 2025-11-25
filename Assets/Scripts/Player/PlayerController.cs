using UnityEngine;

//This will be attached to the player gameobject to control its movement
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController: MonoBehaviour
{
    #region Control Vars
    //control variables
    //a speed value that will control how fast the player moves horizontally
    public float speed = 10f;
    public float initalPowerUpTimer = 5f;
    public float jumpForce = 10f;
    public float groundCheckRadius = 0.02f;
    //public int maxLives = 10;
    //private int _lives = 5;
    private bool isGrounded = false;
    #endregion

    #region Component Ref
    //Component references
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;
    private Animator anim;
    private GroundCheck groundCheck;
    #endregion

    #region State Vars
    //State variables
    private Coroutine jumpForceCoroutine = null;
    private float jumpPowerupTimer = 0f;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get the Rigidbody2D component attached to the same gameobject - we assume that it exists
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        groundCheck = new GroundCheck(col, LayerMask.GetMask("Ground"), groundCheckRadius);

        //Transform based ground check setup - using an empty gameobject as a child of the player to define the ground check position
        //initalize ground check poositon using separate gameobject as a child of the player
        //GameObject newObj = new GameObject("GroundCheck");
        //newObj.transform.SetParent(transform);
        //newObj.transform.localPosition = Vector3.zero;
        //groundCheck = newObj.transform;
        //this is basically the same as doing it in the editor, but we do it here to keep everything self-contained

    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = groundCheck.CheckIsGrounded();

        //grab our horizontal input value - negative button is moving to the left (A/Left Arrow), positive button is moving to the right (D/Right Arrow) - cross platform compatible so it works with keyboard, joystick, etc. -1 to 1 range where zero means no input
        float hValue = Input.GetAxis("Horizontal");
        float vValue = Input.GetAxisRaw("Vertical");
        SpriteFlip(hValue);

        //set the rigidbody's horizontal velocity based on the input value multiplied by our speed - vertical velocity remains unchanged
        rb.linearVelocityX = hValue * speed;

        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                //apply an upward force to the rigidbody when the jump button is pressed
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                anim.SetTrigger("JumpAtk");
            }
        }


        if (Input.GetButtonDown("Fire1") && isGrounded && hValue == 0)
        {
            anim.SetTrigger("Fire");
        }

        //update animator parameters
        anim.SetFloat("hValue", Mathf.Abs(hValue));
        anim.SetBool("isGrounded", isGrounded);
    }

    private void OnValidate() => groundCheck?.UpdateGroundCheckRadius(groundCheckRadius);

    private void SpriteFlip(float hValue)
    {
        //we can use the (hValue < 0) expression to set flipX directly - hValue is negative when moving left, so flipX should be true
        if (hValue != 0)
            sr.flipX = (hValue < 0);

        //flip the sprite based on the direction we are moving - flipX is true when the sprite is facing left
        //if (sr.flipX && hValue > 0 || !sr.flipX && hValue < 0)
        //{
        //    sr.flipX = !sr.flipX;
        //}
    }

    //dynamic rigidbody collides with another dynamic or static collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            TakeDamage(1);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Collided With: " + collision.gameObject.name);
    }
    //dynamic rigidbody collides with another dynamic or static collider

    //These functions are called when a trigger collider is entered, stayed in, or exited - they don't really have any limits on what they can interact with
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Squish") && rb.linearVelocityY < 0)
        {
            collision.GetComponentInParent<BaseEnemy>().TakeDamage(0, DamageType.JumpedOn);
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    public void TakeDamage(int livesLost)
    {
        GameManager.Instance.lives -= livesLost;
    }

    public void IncreaseGravity() => rb.gravityScale = 5f;

    #region Powerup Functions
    public void ApplyJumpForcePowerup()
    {
        if (jumpForceCoroutine != null)
        {
            StopCoroutine(jumpForceCoroutine);
            jumpForceCoroutine = null;
            jumpForce = 7;
        }

        jumpForceCoroutine = StartCoroutine(JumpForceChange());
    }

    System.Collections.IEnumerator JumpForceChange()
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
    #endregion

}