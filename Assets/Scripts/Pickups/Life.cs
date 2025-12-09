using UnityEngine;

//Life derivers from pickup which is a monobehaviour. We derive from pickup so that we can have access to the onpickup function that will be our specific effect when we pickup this item.
[RequireComponent(typeof(Rigidbody2D))]
public class Life: Pickup
{
    public int livesToAdd = 1;
    public Collider2D childCollider;

    Rigidbody2D rb;
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = new Vector2(-2, 2);
    }

    private void Update()
    {
        rb.linearVelocity = new Vector2(-2, rb.linearVelocity.y);
    }

    public override void OnPickup(GameObject player)
    {
        childCollider.enabled = false;
        GameManager.Instance.lives += livesToAdd;
    }
}
