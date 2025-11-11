// Libraries
using UnityEngine;

public class SimplePickups : MonoBehaviour
{
    // Types of pickups
    public enum PickupType
    {
        Life,               // Increases player's life
        Powerup,            // Grants player a powerup
        Coin                // Increases player's coin count
    }

    // Type of this pickup
    public PickupType pickupType;

    // Called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider belongs to the player
        if (collision.CompareTag("Player"))
        {
            // Get the PlayerController component from the player
            PlayerController pc = collision.GetComponent<PlayerController>();
            // Apply the effect based on the pickup type
            switch (pickupType)
            {
                case PickupType.Life:
                    // Increase player's life
                    pc.lives++;
                    break;
                case PickupType.Powerup:
                    // Grant player a powerup
                    pc.ApplyJumpForcePowerup();
                    break;
                case PickupType.Coin:
                    // Increase player's coin count
                    break;
            }
            // Destroy the pickup after collection
            Destroy(gameObject);
        }
    }
}
