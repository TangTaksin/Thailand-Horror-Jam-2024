using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    public bool isOccupied = false; // Check if the hiding spot is occupied
    public string hidingMessage = "Hiding..."; // Message displayed when hiding

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.SetCurrentHidingSpot(this); // Set the current hiding spot when the player enters
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // You can add logic here to continuously interact with the hiding spot
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null && !isOccupied)
            {
                playerController.SetCurrentHidingSpot(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ClearCurrentHidingSpot(); // Clear the current hiding spot when the player exits
            }
        }
    }
}
