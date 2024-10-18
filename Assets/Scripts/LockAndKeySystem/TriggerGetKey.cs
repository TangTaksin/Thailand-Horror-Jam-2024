using UnityEngine;

public class TriggerGetKey : MonoBehaviour, IInteractable
{
    // Array of string identifiers for the required keys
    public string[] requiredKeys; // The keys required to trigger the event
    public string keyToGive = "Key B"; // The key to give if the player has all the required keys
    public FeedbackManager feedbackManager; // Reference to the FeedbackManager

    public Vector3 position => transform.position; // Expression-bodied member for simplicity

    [SerializeField] private bool _isInteractable = true;
    public bool isInteractable
    {
        get => _isInteractable;
        set => _isInteractable = value;
    }

    // Interact with the lock
    public void Interact(object interacter)
    {
        if (!isInteractable) return;

        if (interacter is PlayerController playerController)
        {
            // Check if the player has all the required keys
            if (PlayerHasAllRequiredKeys())
            {
                // Give the key to the player
                PlayerInventory.instance.AddKey(keyToGive);
                // Highlight keyToGive in red
                feedbackManager.ShowFeedback("คุณได้รับ <color=red>" + keyToGive + "</color> !"); // "You have been given <key>!"

                // Optional: Destroy this trigger after giving the key
                Destroy(gameObject);
            }
            else
            {
                // Inform the player of the missing keys
                string missingKeys = GetMissingKeys();
                feedbackManager.ShowFeedback("ต้องการ <color=red>" + missingKeys + "</color> เพื่อดำเนินการต่อ."); // "You need <keys> to proceed."
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerHasAllRequiredKeys())
            {
                // Show the message to press "W" to receive the key if no keys are missing
                feedbackManager.ShowFeedback("กด W เพื่อรับ <color=red>" + keyToGive + "</color> !");
                isInteractable = true; // Enable interaction if keys are present
            }
            else
            {
                // Show the missing keys message
                string missingKeys = GetMissingKeys();
                feedbackManager.ShowFeedback("ต้องการ <color=red>" + missingKeys + "</color> เพื่อดำเนินการต่อ."); // "You need <keys> to proceed."
                isInteractable = false; // Disable interaction if keys are missing
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (PlayerHasAllRequiredKeys())
        {
            // Show the message to press "W" to receive the key if no keys are missing
            feedbackManager.ShowFeedback("กด W เพื่อรับ <color=red>" + keyToGive + "</color> !");
            isInteractable = true; // Enable interaction if keys are present
        }
        else
        {
            // Show the missing keys message
            string missingKeys = GetMissingKeys();
            feedbackManager.ShowFeedback("ต้องการ <color=red>" + missingKeys + "</color> เพื่อดำเนินการต่อ."); // "You need <keys> to proceed."
            isInteractable = false; // Disable interaction if keys are missing
        }
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
}
