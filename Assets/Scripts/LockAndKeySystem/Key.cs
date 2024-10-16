using System.Collections;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public string keyID; // Unique identifier for this key
    public FeedbackManager feedbackManager; // Reference to the FeedbackManager

    public Vector3 position => transform.position; // Use expression-bodied member for simplicity

    [SerializeField] private bool _isInteractable;
    public bool isInteractable
    {
        get => _isInteractable; // Use expression-bodied member
        set => _isInteractable = value;
    }

    public void Interact(object interacter)
    {
        if (!isInteractable) return; // Ensure the key can only be interacted with once

        // Add key to player's inventory
        PlayerInventory.instance.AddKey(keyID);

        // Show feedback to the player with highlighted keyID
        ShowFeedbackWithHighlight(keyID);

        // Hide the key's sprite renderer and start destruction coroutine
        HideSpriteRenderer();
        StartCoroutine(DestroyKeyAfterDelay(2.5f));

        // Mark the key as not interactable
        isInteractable = false;
    }

    private void ShowFeedbackWithHighlight(string keyID)
    {
        // Create feedback message with red color for keyID
        string feedbackMessage = $" ได้เก็บ <color=red>{keyID}</color> มาแล้ว น่าจะเป็นของสำคัญ";
        feedbackManager.ShowFeedback(feedbackMessage);
    }

    private void HideSpriteRenderer()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false; // Hide the sprite
        }
    }

    private IEnumerator DestroyKeyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        Destroy(gameObject); // Then destroy the key object
    }
}
