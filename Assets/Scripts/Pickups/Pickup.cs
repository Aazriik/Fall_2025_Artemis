using UnityEngine;
using UnityEngine.Audio;

//this is an abstract class - abstract classes are not able to be instantiated - but they are instead used for templatating a child object. We can leverage this to create a polymorphic OnPickup function that will behave based on the class that is implementing it rather than the base class onpickup function.
public abstract class Pickup: MonoBehaviour
{
    public AudioMixerGroup sfxMixerGroup;
    public AudioClip pickupSound;

    protected AudioSource source;
    public float lifetime = 0.2f;

    public virtual void Start()
    {
        if (pickupSound != null)
        {
            TryGetComponent<AudioSource>(out source);
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
                Debug.LogWarning("Dynamically adding Audio Source");
            }

            source.outputAudioMixerGroup = sfxMixerGroup;
        }
    }

    //Function to be called when the player collides with the pickup
    abstract public void OnPickup(GameObject player);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the player collided with the pickup
        if (collision.CompareTag("Player"))
        {
            //Call the OnPickup function and pass the player object
            OnPickup(collision.gameObject);

            //Play pickup sound
            source.PlayOneShot(pickupSound);

            //Disable the sprite and collider
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

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

            //Play pickup sound
            source.PlayOneShot(pickupSound);

            //Disable the sprite and collider
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            //Destroy the pickup object
            Destroy(gameObject, lifetime);
        }
    }
}
