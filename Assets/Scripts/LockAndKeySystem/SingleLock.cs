using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleLock : MonoBehaviour
{
    public string requiredKeyID;  // Key ID required to unlock this door (changed to string)
    public GameObject door;        // Reference to the door GameObject
    public FeedbackManager feedbackManager; // Reference to the FeedbackManager

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the player has the required key
            if (PlayerInventory.instance.HasKey(requiredKeyID)) // Use string key
            {
                // Unlock the door
                Unlock();
            }
            else
            {
                // Show feedback using the FeedbackManager
                feedbackManager.ShowFeedback("คุณต้องการกุญแจ " + requiredKeyID + " เพื่อปลดล็อกประตูนี้."); // "You need key <keyID> to unlock this door."
            }
        }
    }

    private void Unlock()
    {
        // Perform unlock action (for example, destroy or disable the locked door)
        Debug.Log("ประตูถูกปลดล็อก!"); // "Door unlocked!"
        feedbackManager.ShowFeedback("ประตูถูกปลดล็อก!"); // Show feedback for unlocking the door
        door.SetActive(false); // Disabling the door to simulate unlocking

        // Optionally, play sound or animation here

        // Destroy lock or make it inactive
        Destroy(gameObject);
    }
}
