using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public GameObject objectToDestroy;   // Object to destroy after collecting the item
    public GameObject objectToActivate;  // Object to activate after collecting the item

    [SerializeField] private bool _isInteractable = true; // Default is true for interactable
    public bool isInteractable
    {
        get => _isInteractable;
        set => _isInteractable = value;
    }

    // Implementing the position property from IInteractable
    public Vector3 position => transform.position; // Get the position of the item in world space

    // The interaction method that gets called when the player interacts with the item
    public void Interact(object interacter)
    {
        // Check if item is interactable before doing anything
        if (!isInteractable)
        {
            Debug.Log("Item is not interactable.");
            return;
        }

        // Ensure the interacter is the player (optional: type check for your player class)
        if (interacter is PlayerController player)
        {
            Debug.Log("Interacted with item.");

            // Handle destruction of objectToDestroy
            if (objectToDestroy != null)
            {
                Destroy(objectToDestroy);
                Debug.Log("Destroyed object: " + objectToDestroy.name);
            }

            // Handle activation of objectToActivate
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
                Debug.Log("Activated object: " + objectToActivate.name);
            }

            // Set the item as non-interactable to prevent multiple interactions
            isInteractable = false;

            // Optionally: Hide the item (or destroy it) after interaction
            HideItem();
        }
        else
        {
            Debug.Log("Non-player interacter tried to interact.");
        }
    }

    // Optional: Hide the item from the world (disable its sprite renderer or the whole object)
    private void HideItem()
    {
        // Option 1: Disable the item object completely
        gameObject.SetActive(false);

        // Option 2: Alternatively, hide only the sprite renderer to keep the object active in the scene
        // SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        // if (spriteRenderer != null) spriteRenderer.enabled = false;
    }
}
