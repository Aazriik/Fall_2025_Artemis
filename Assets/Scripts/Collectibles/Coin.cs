using UnityEngine;
[RequireComponent( typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Here you can add code to increase the player's score or coin count
            Debug.Log("Coin collected!");
            // Destroy the coin object
            Destroy(gameObject);
        }
    }
}
