using UnityEngine;

public class TriggerGetKey : MonoBehaviour
{
    // String identifiers for the required key and the key to give
    public string requiredKey = "Key A"; // The key required to trigger the event
    public string keyToGive = "Key B"; // The key to give if the player has the required key
    public FeedbackManager feedbackManager; // Reference to the FeedbackManager

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Check if the player has the required key using the string identifier
            if (PlayerInventory.instance.HasKey(requiredKey))
            {
                GiveKeyToPlayer();
            }
            else
            {
                feedbackManager.ShowFeedback("ต้องการ " + requiredKey + " เพื่อดำเนินการต่อ."); // "You need <key> to proceed."
            }
        }
    }

    void GiveKeyToPlayer()
    {
        // Add the new key to the player's inventory using the string identifier
        PlayerInventory.instance.AddKey(keyToGive);
        feedbackManager.ShowFeedback("คุณได้รับ " + keyToGive + " !"); // "You have been given <key>!"

        // Optional: Destroy this trigger after giving the key
        Destroy(gameObject);
    }
}
