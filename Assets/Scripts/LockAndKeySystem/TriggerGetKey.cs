using UnityEngine;

public class TriggerGetKey : MonoBehaviour
{
    // Array of string identifiers for the required keys
    public string[] requiredKeys; // The keys required to trigger the event
    public string keyToGive = "Key B"; // The key to give if the player has all the required keys
    public FeedbackManager feedbackManager; // Reference to the FeedbackManager

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Check if the player has all the required keys
            if (PlayerHasAllRequiredKeys())
            {
                GiveKeyToPlayer();
            }
            else
            {
                string missingKeys = GetMissingKeys();
                // Highlight missingKeys in red
                feedbackManager.ShowFeedback("ต้องการ <color=red>" + missingKeys + "</color> เพื่อดำเนินการต่อ."); // "You need <keys> to proceed."
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        string missingKeys = GetMissingKeys();
        // Highlight missingKeys in red
        feedbackManager.ShowFeedback("ต้องการ <color=red>" + missingKeys + "</color> เพื่อดำเนินการต่อ."); // "You need <keys> to proceed."
    }

    // Check if the player has all the required keys
    bool PlayerHasAllRequiredKeys()
    {
        foreach (string key in requiredKeys)
        {
            if (!PlayerInventory.instance.HasKey(key))
            {
                return false; // If the player is missing any required key, return false
            }
        }
        return true; // Player has all required keys
    }

    // Get a string listing all the missing keys
    string GetMissingKeys()
    {
        string missingKeys = "";
        foreach (string key in requiredKeys)
        {
            if (!PlayerInventory.instance.HasKey(key))
            {
                if (missingKeys != "") missingKeys += ", "; // Add comma between missing keys
                missingKeys += key; // Add the missing key to the string
            }
        }
        return missingKeys;
    }

    // Give the new key to the player
    void GiveKeyToPlayer()
    {
        // Add the new key to the player's inventory using the string identifier
        PlayerInventory.instance.AddKey(keyToGive);
        // Highlight keyToGive in red
        feedbackManager.ShowFeedback("คุณได้รับ <color=red>" + keyToGive + "</color> !"); // "You have been given <key>!"

        // Optional: Destroy this trigger after giving the key
        Destroy(gameObject);
    }
}
