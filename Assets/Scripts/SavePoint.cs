using UnityEngine;
using TMPro; // Include the TextMeshPro namespace

public class SavePoint : MonoBehaviour, IInteractable
{
    private bool playerInRange = false;

    // Reference to the FeedbackManager
    public FeedbackManager feedbackManager;

    // Reference to the SpriteRenderer component
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Color to change the sprite to when saved
    public Color savedColor = Color.green; // Change this to your desired color

    // Color to revert to when entering the save point
    public Color defaultColor = Color.white; // Change this to your desired default color

    public Vector3 position => transform.position; // Expression-bodied member for position

    [SerializeField] private bool _isInteractable = true; // Default is true for interactable
    public bool isInteractable
    {
        get => _isInteractable;
        set => _isInteractable = value;
    }

    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Ensure the sprite is set to the default color on start
        if (spriteRenderer != null)
        {
            spriteRenderer.color = defaultColor;
        }
    }

    // Called when interacting with the save point
    public void Interact(object interacter)
    {
        if (!isInteractable || !playerInRange) return; // Ensure player is in range and object is interactable

        if (interacter is PlayerController playerController)
        {
            // Save the game when the player interacts
            SaveSystem.SavePlayer(playerController.gameObject);

            // Use FeedbackManager to show save feedback
            feedbackManager?.ShowFeedback("Game saved!");

            // Change sprite color to indicate save
            ChangeSpriteColor(savedColor); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;

            // Use FeedbackManager to show interaction prompt
            feedbackManager?.ShowFeedback("Press 'W' to save.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;

            // Clear the feedback message when player leaves the save point
            feedbackManager?.HideFeedback();
        }
    }

    // Method to change the sprite color
    private void ChangeSpriteColor(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color; // Change the sprite color
        }
    }
}
