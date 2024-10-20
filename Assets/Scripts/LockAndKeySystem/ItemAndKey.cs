using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAndKey : MonoBehaviour
{
    // Key-related variables
    public string keyID; // Unique identifier for the key
    public FeedbackManager feedbackManager; // Reference to the FeedbackManager for feedback

    // Item-related variables
    public GameObject objectToDestroy; // Object to destroy after collecting the item
    public GameObject objectToActivate; // Object to activate after collecting the item

    // Interactability settings
    [SerializeField] private bool _isInteractable = true; // Default is true for interactable
    public bool isInteractable
    {
        get => _isInteractable;
        set => _isInteractable = value;
    }

    // Position property from IInteractable
    public Vector3 position => transform.position; // Get the position of the object in world space

    // The interaction method that gets called when the player interacts with the item/key
    public void Interact(object interacter)
    {
        if (!isInteractable) return; // Ensure the object is interactable

        if (interacter is PlayerController)
        {
            // Handle key interaction
            HandleKeyInteraction();

            // Handle item interaction
            HandleItemInteraction();
        }
    }

    // Handle the logic for key interaction (like collecting the key)
    private void HandleKeyInteraction()
    {
        if (!string.IsNullOrEmpty(keyID))
        {
            // Add key to player's inventory
            PlayerInventory.instance.AddKey(keyID);

            // Show feedback to the player with highlighted keyID
            ShowFeedbackWithHighlight(keyID);

            // Hide the key's sprite renderer and start destruction coroutine
            HideSpriteRenderer();
            isInteractable = false;
            StartCoroutine(DestroyObjectAfterDelay(1.5f));
        }
    }

    // Show feedback with a highlighted keyID
    private void ShowFeedbackWithHighlight(string keyID)
    {
        string feedbackMessage = $"ได้เก็บ <color=red>{keyID}</color> มาแล้ว น่าจะเป็นของสำคัญ";
        if (feedbackManager != null)
        {
            feedbackManager.ShowFeedback(feedbackMessage);
        }
        else
        {
            Debug.LogWarning("FeedbackManager is not assigned.");
        }
    }

    // Hide the sprite renderer (used for key's sprite)
    private void HideSpriteRenderer()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false; // Hide the sprite
        }
    }

    // Coroutine to destroy the object after a delay
    private IEnumerator DestroyObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    // Handle the logic for item interaction (like activating or destroying objects)
    private void HandleItemInteraction()
    {
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
            Debug.Log("Destroyed object: " + objectToDestroy.name);
        }

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            Debug.Log("Activated object: " + objectToActivate.name);
        }

        // Disable further interaction after the item is interacted with
        isInteractable = false;
    }
}
