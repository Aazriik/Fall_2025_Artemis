using UnityEngine;

public class TurretEnemy: BaseEnemy
{
    [SerializeField] private float fireRate = 2.0f;
    private float timeSinceLastFire = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        if (fireRate <= 0)
        {
            Debug.LogError("Fire rate must be greater than zero, set to default value of 2");
            fireRate = 2.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Idle"))
        {
            //trigger fire animation logic
            if (Time.time >= timeSinceLastFire + fireRate)
            {
                anim.SetTrigger("Fire");
                timeSinceLastFire = Time.time;
            }
        }
    }
}
