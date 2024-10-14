using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player enters the hiding spot
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.EnterHidingSpot(this); // Notify the player that they can hide
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the player exits the hiding spot
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ExitHidingSpot(this); // Notify the player that they can no longer hide
            }
        }
    }
}
