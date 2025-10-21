using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables
    public float speed = 10f;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float hValue = Input.GetAxis("Horizontal");
        rb.linearVelocityX = hValue * speed;

    }
}