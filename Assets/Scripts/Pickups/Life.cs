using UnityEngine;

//Life derivers from pickup which is a monobehaviour. We derive from pickup so that we can have access to the onpickup function that will be our specific effect when we pickup this item.
[RequireComponent(typeof(Rigidbody2D))]
public class Life: Pickup
{
    // Variables
    public int livesToAdd = 1;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = new Vector2(-2, 2);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(-2, rb.linearVelocity.y);
    }

    public override void OnPickup(GameObject player) => player.GetComponent<PlayerController>().lives += livesToAdd;
}
