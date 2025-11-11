using UnityEngine;

//this is an abstract class - abstract classes are not able to be instantiated - but they are instead used for templatating a child object. We can leverage this to create a polymorphic OnPickup function that will behave based on the class that is implementing it rather than the base class onpickup function.
public abstract class Pickup: MonoBehaviour
{
    public float lifetime = 0.2f;

    //Function to be called when the player collides with the pickup
    abstract public void OnPickup(GameObject player);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the player collided with the pickup
        if (collision.CompareTag("Player"))
        {
            //Call the OnPickup function and pass the player object
            OnPickup(collision.gameObject);
            //Destroy the pickup object
            Destroy(gameObject, lifetime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if the player collided with the pickup
        if (collision.gameObject.CompareTag("Player"))
        {
            //Call the OnPickup function and pass the player object
            OnPickup(collision.gameObject);
            //Destroy the pickup object
            Destroy(gameObject, lifetime);
        }
    }
}
