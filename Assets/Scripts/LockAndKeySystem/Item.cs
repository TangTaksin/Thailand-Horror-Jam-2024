using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public GameObject objectToDestroy; // Object to destroy after collecting the item
    public GameObject objectToActivate; // Object to activate after collecting the item

    [SerializeField] private bool _isInteractable = true; // Default is true for interactable
    public bool isInteractable
    {
        get => _isInteractable;
        set => _isInteractable = value;
    }

    // Implementing the position property from IInteractable
    public Vector3 position => transform.position; // Get the position of the item in world space

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the player entered the trigger
        {
            Debug.Log("Player in range, ready to interact.");
            // Optionally show UI or prompt
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Player leaves the trigger
        {
            Debug.Log("Player out of range.");
            // Optionally hide UI or prompt
        }
    }

    // The interaction method that gets called when the player interacts with the item
    public void Interact(object interacter)
    {
        if (!isInteractable) return; // Ensure the object is interactable

        if (interacter is PlayerController)
        {
            Debug.Log("Interacted with item.");

            // If there's a referenced object to destroy, destroy it
            if (objectToDestroy != null)
            {
                Destroy(objectToDestroy);
                Debug.Log("Destroyed object: " + objectToDestroy.name);
            }

            // If there's a referenced object to activate, activate it
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
                Debug.Log("Activated object: " + objectToActivate.name);
            }

            // Optionally, you can destroy the item itself after interaction
            Destroy(gameObject); // Remove the item itself after interaction
            Debug.Log("Item collected and destroyed.");
        }
    }
}
