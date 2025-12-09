using UnityEngine;

public class Shoot : MonoBehaviour
{
    // Variables
    // Velocity applied to the projectile when fired
    [Header("Shoot Settings")]
    [SerializeField] private Vector2 initialShotVelocity = Vector2.zero;
    // Projectile spawn points and prefab
    [Header("Projectile Settings")]
    [SerializeField] private Transform spawnPointRight;
    [SerializeField] private Transform spawnPointLeft;
    [SerializeField] private Projectile projectilePrefab;

    [Header("Sound Effects")]
    public AudioClip fireballSFX;

    // Component Refs
    private SpriteRenderer sr;
    private AudioSource source;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the SpriteRenderer component
        sr = GetComponent<SpriteRenderer>();

        // Get the AudioSource component
        source = GetComponent<AudioSource>();

        // Error Checking
        if (initialShotVelocity == Vector2.zero)
        {
            // Set a default value for initialShotVelocity
            initialShotVelocity = new Vector2(10f, 0f);
            // Log a warning if it was not set
            Debug.LogWarning("Initial shot velocity not set on Shoot component of " + gameObject.name + ", defaulting to " + initialShotVelocity);
        }
        // Check if spawn points and projectile prefab are set
        if (spawnPointLeft == null || spawnPointRight == null || projectilePrefab == null)
        {
            // Log an error if any are not set
            Debug.LogError("Shoot: Spawn points or projectile is not set on Shoot component of ");
        }
    }

    // Method to fire a projectile
    public void Fire()
    {
        // Instantiate the projectile at the appropriate spawn point based on the sprite's facing direction
        Projectile curProjectile;
        // If not flipped, shoot to the right
        if (!sr.flipX)
        {
            // Instantiate projectile at right spawn point
            curProjectile = Instantiate(projectilePrefab, spawnPointRight.position, Quaternion.identity);
            // Set its velocity to the initial shot velocity
            curProjectile.SetVelocity(initialShotVelocity);
        }
        // If flipped, shoot to the left
        else
        {
            // Instantiate projectile at left spawn point
            curProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, Quaternion.identity);
            // Set its velocity to the negative initial shot velocity on the x-axis
            curProjectile.SetVelocity(new Vector2(-initialShotVelocity.x, initialShotVelocity.y));
        }
    }
}
