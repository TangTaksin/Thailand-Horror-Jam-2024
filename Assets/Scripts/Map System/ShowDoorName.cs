using System.Collections;
using UnityEngine;

public class ShowDisplayName : MonoBehaviour
{
    public string displayName; // Name of the door or NPC to display
    public FeedbackManager feedbackManager; // Reference to the FeedbackManager
    public bool isNPC; // Boolean to determine if the feedback is for an NPC
    public bool isItem; // Boolean to determine if the feedback is for an item
    public bool displayable;

    private bool playerInRange = false; // Tracks if the player is within the trigger's range

    public void SetDisplayable(bool value)
    {
        displayable = value;
    }

    // Check if the player enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!displayable)
            return;

        if (collision.CompareTag("Player") && !playerInRange)
        {
            playerInRange = true; // Set playerInRange to true

            // Show appropriate feedback based on whether it's an NPC, door, or item
            string feedbackMessage = GenerateFeedbackMessage();
            feedbackManager.ShowFeedback(feedbackMessage);
        }
    }

    // Check when the player leaves the trigger area
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false; // Reset the flag
            feedbackManager.HideFeedback(); // Hide feedback when player leaves
        }
    }

    // Generate the feedback message based on the type of interaction
    private string GenerateFeedbackMessage()
    {
        if (isNPC)
        {
            return $"=== {displayName} ===\nPress <color=green>W</color> to Interact";
        }
        else if (isItem)
        {
            return $"=== {displayName} ===\nPress <color=green>W</color> to Pick Up";
        }
        else // Assuming it's a door if neither is true
        {
            return $"=== {displayName} ===\nPress <color=green>W</color> to Enter";
        }
    }
}
