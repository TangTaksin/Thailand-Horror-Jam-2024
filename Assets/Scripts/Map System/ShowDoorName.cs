using UnityEngine;

public class ShowDisplayName : MonoBehaviour
{
    [Header("Display Settings")]
    public string displayName; // Name of the door or NPC to display
    public bool isNPC; // Boolean to determine if the feedback is for an NPC
    public bool isItem; // Boolean to determine if the feedback is for an item
    public bool isInfo; // Boolean to determine if the feedback is informational

    [Header("Interaction Settings")]
    public bool displayable = true; // Can the object display feedback?

    // Cached References
    private FeedbackManager feedbackManager;
    private bool playerInRange = false; // Tracks if the player is within the trigger's range

    private void Awake()
    {
        // Cache the FeedbackManager to avoid repeated GetComponent calls
        feedbackManager = FindObjectOfType<FeedbackManager>();
    }

    public void SetDisplayable(bool value)
    {
        displayable = value;
    }

    // Check if the player enters the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!displayable || playerInRange || !collision.CompareTag("Player")) 
            return;

        playerInRange = true; // Set playerInRange to true

        // Show feedback based on the type of interaction (NPC, Item, or Door)
        feedbackManager.ShowFeedback(GenerateFeedbackMessage());
    }

    // Check when the player leaves the trigger area
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) 
            return;

        playerInRange = false; // Reset the flag
        feedbackManager.HideFeedback(); // Hide feedback when player leaves
    }

    // Generate the feedback message based on the type of interaction
    private string GenerateFeedbackMessage()
    {
        string interactionType = isNPC ? "Interact" : isItem ? "Pick Up" : isInfo ? "" : "Enter";

        return isInfo ? $"=== {displayName} ===" :
                        $"=== {displayName} ===\nPress <color=green>W</color> to {interactionType}";
    }
}
