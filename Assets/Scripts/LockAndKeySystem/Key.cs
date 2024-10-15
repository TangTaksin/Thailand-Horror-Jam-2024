using UnityEngine;

public class Key : MonoBehaviour
{
    public string keyID; // Unique identifier for this key
    public FeedbackManager feedbackManager; // Reference to the FeedbackManager

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the collider is the Player
        {
            // Add key to player's inventory
            PlayerInventory.instance.AddKey(keyID); // Use the string identifier for the key

            // Show feedback to the player
            feedbackManager.ShowFeedback(keyID + " ถูกเก็บแล้ว!");

            // Destroy the key after collecting
            Destroy(gameObject);
        }
    }
}
