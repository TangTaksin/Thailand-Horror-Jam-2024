using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Early exit if the object is not the player
        if (!collision.CompareTag("Player")) return;

        // Try to get the PlayerController component
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player == null) return;

        // Notify the player that they can hide
        player.EnterHidingSpot(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Early exit if the object is not the player
        if (!collision.CompareTag("Player")) return;

        // Try to get the PlayerController component
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player == null) return;

        // Notify the player that they can no longer hide
        player.ExitHidingSpot(this);
    }
}
