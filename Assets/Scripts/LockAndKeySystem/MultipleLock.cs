using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleLock : MonoBehaviour
{
    public string[] requiredKeyIDs; // Array of required key IDs for this lock
    public GameObject door; // Reference to the door GameObject
    public FeedbackManager feedbackManager; // Reference to FeedbackManager for showing messages

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (HasAllKeys())
            {
                // Unlock the door and give feedback
                Unlock();
                feedbackManager.ShowFeedback("ประตูถูกปลดล็อคแล้ว!"); // "The door is unlocked!"
            }
            else
            {
                // Provide feedback on missing keys
                string missingKeys = GetMissingKeys();
                feedbackManager.ShowFeedback("คุณต้องการกุญแจ: " + missingKeys + " เพื่อปลดล็อคประตู."); // "You need keys: <missingKeys> to unlock the door."
            }
        }
    }

    private bool HasAllKeys()
    {
        // Check if the player has all required keys
        foreach (string keyID in requiredKeyIDs)
        {
            if (!PlayerInventory.instance.HasKey(keyID))
            {
                return false;
            }
        }
        return true;
    }

    private string GetMissingKeys()
    {
        // Build a list of missing keys
        string missingKeys = "";
        foreach (string keyID in requiredKeyIDs)
        {
            if (!PlayerInventory.instance.HasKey(keyID))
            {
                if (missingKeys != "") missingKeys += ", "; // Add comma between keys
                missingKeys += keyID; // Add the missing key ID
            }
        }
        return missingKeys;
    }

    private void Unlock()
    {
        // Perform unlock action
        Debug.Log("Door unlocked!");
        door.SetActive(false); // Disabling the door to simulate unlocking

        // Optionally, play sound or animation here

        // Destroy or deactivate the lock after unlocking
        Destroy(gameObject);
    }
}
