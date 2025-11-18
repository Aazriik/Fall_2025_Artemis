using UnityEngine;

//abstract class that cannot be instantiated but inherited from.
[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public abstract class BaseEnemy: MonoBehaviour
{
    //first keyword in a variable decleration is the access modifier (if you don't put any access modifier, it defaults to protected)
    //public - a variable that is publically accessable from other scripts. Unless the variable is static you do need an actual reference to the instantiated class
    //private - a variable that is only accesable to the class it is from - you can only use it in the script that defined it - cannot be inherited
    //protected - a variable that is only accessable to the class that defined it and it's children - this variable can inherited
    protected SpriteRenderer sr;
    protected Animator anim;
    protected int health;

    public int maxHealth = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //virtual functions are functions that can be overriden in child classes - we override them and can either ensure base class behaviour is called or not
    //virtual functions must be public or protected as they are meant to be used by child classes. You cannot make a private vitual function because it doesn't make sense.
    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (maxHealth <= 0)
        {
            Debug.LogError("Max health value should be a value greater than zero, setting to a default value of 5");
            maxHealth = 5;
        }

        health = maxHealth;
    }

    public virtual void TakeDamage(int damageValue, DamageType damageType = DamageType.Default)
    {
        health -= damageValue;

        if (health <= 0)
        {
            anim.SetTrigger("Death");

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject, 0.5f);
            }
            else
            {
                Destroy(gameObject, 0.5f);
            }
        }
    }
}

public enum DamageType
{
    Default,
    JumpedOn
}