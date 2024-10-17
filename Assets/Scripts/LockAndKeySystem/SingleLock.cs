using System.Collections;
using UnityEngine;

public class SingleLock : MonoBehaviour, IInteractable
{
    [Header("Lock Settings")]
    public string requiredKeyID; // Key ID required to unlock this door
    public GameObject door; // Reference to the door GameObject
    public FeedbackManager feedbackManager; // Reference to the FeedbackManager
    public Vector3 position => transform.position; // Expression-bodied member for simplicity

    [SerializeField] private bool _isInteractable;
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
            AttemptUnlock(playerController.gameObject);
        }
    }

    // Handle the player entering the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && PlayerInventory.instance.HasKey(requiredKeyID))
        {
            feedbackManager.ShowFeedback("กด <color=green>W</color> เพื่อปลดล็อก");
        }
        else
        {
            
            feedbackManager.ShowFeedback("กด <color=green>W</color> เพื่อพยายามกระโดดข้าม");

        }
    }

    // Attempt to unlock the door based on the player's inventory
    private void AttemptUnlock(GameObject player)
    {
        if (PlayerInventory.instance.HasKey(requiredKeyID)) // Check if player has the required key
        {
            Unlock();
        }
        else
        {
            feedbackManager.ShowFeedback($"ต้องการ <color=red>{requiredKeyID}</color> เพื่อปลดล็อก."); // Show key requirement feedback
        }
    }

    // Unlock the door
    private void Unlock()
    {
        feedbackManager.ShowFeedback("ปลดล็อกแล้ว!"); // Show unlock feedback
        door.SetActive(false); // Disable the door to simulate unlocking

        Destroy(gameObject); // Destroy the lock
    }
}
