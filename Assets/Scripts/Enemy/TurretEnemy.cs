using UnityEngine;

public class TurretEnemy: BaseEnemy
{
    [SerializeField] private float fireRate = 2.0f;
    private float timeSinceLastFire = 0;
    private bool playerInRange = false;

    private PlayerController playerRef;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        if (fireRate <= 0)
        {
            Debug.LogError("Fire rate must be greater than zero, set to default value of 2");
            fireRate = 2.0f;
        }

        GameManager.Instance.OnPlayerSpawned += (PlayerController playerInstance) => playerRef = playerInstance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!playerRef) return;

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Idle"))
        {
            //trigger fire animation logic
            if (Time.time >= timeSinceLastFire + fireRate && playerInRange)
            {
                anim.SetTrigger("Fire");
                timeSinceLastFire = Time.time;
            }
        }
    }

    void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            sr.color = Color.red;

            // Flip to the player's direction
            if (collision.transform.position.x < transform.position.x)
                sr.flipX = true;
            else
                sr.flipX = false;
        }
    }

    void OnTriggerExit2D (Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            sr.color = Color.white;
        }
    }
}
