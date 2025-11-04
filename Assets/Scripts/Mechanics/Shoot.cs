using UnityEngine;

public class Shoot : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private Vector2 initialShotVelocity = Vector2.zero;
    [SerializeField] private Transform spawnPointRight;
    [SerializeField] private Transform spawnPointLeft;
    [SerializeField] private Projectile projectilePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (initialShotVelocity == Vector2.zero)
        {
            initialShotVelocity = new Vector2(10f, 0f);
            Debug.LogError("Shoot: initialShotVelocity is not set.");
        }

        if (spawnPointLeft == null || spawnPointRight == null || projectilePrefab == null)
        {
            Debug.LogError("Shoot: Spawn points or projectile is not set on Shoot component of ");
        }
    }

    public void Fire()
    {
        Projectile curProjectile;
        if (!sr.flipX)
        {
            curProjectile = Instantiate(projectilePrefab, spawnPointRight.position, Quaternion.identity);
            curProjectile.SetVelocity(initialShotVelocity);
        }
        else
        {
            curProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, Quaternion.identity);
            curProjectile.SetVelocity(new Vector2(-initialShotVelocity.x, initialShotVelocity.y));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
