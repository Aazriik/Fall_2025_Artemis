using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    // Lifetime of the projectile in seconds
    [SerializeField, Range(0.5f, 10.0f)] private float lifetime = 10f;

    // Start is called before the first frame update
    // Destroys the projectile after its lifetime expires
    void Start() => Destroy(gameObject, lifetime);

    // Sets the velocity of the projectile's Rigidbody2D
    public void SetVelocity(Vector2 velocity) => GetComponent<Rigidbody2D>().linearVelocity = velocity;
}
