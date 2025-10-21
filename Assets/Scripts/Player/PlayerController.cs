using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{
    #region Variables

    // Movement
    public float moveSpeed = 10f;           // Player movement speed
    Rigidbody2D rb;                         // Reference to the player's Rigidbody2D

    #endregion

    // Start is called once at creation
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float hValue = Input.GetAxis("Horizontal");
        rb.linearVelocityX = hValue * moveSpeed;
    }

}