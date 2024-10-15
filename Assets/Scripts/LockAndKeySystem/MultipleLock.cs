using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleLock : MonoBehaviour
{
    public string[] requiredKeyIDs; // Array of required key IDs for this lock (changed to string)
    public GameObject door; // Reference to the door GameObject

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (HasAllKeys())
            {
                // Unlock the door
                Unlock();
            }
            else
            {
                Debug.Log("You don't have all the keys to unlock this door.");
            }
        }
    }

    private bool HasAllKeys()
    {
        // Check if the player has all required keys
        foreach (string keyID in requiredKeyIDs)
        {
            if (!PlayerInventory.instance.HasKey(keyID)) // Check against string keys
            {
                return false;
            }
        }
        return true;
    }

    private void Unlock()
    {
        // Perform unlock action
        Debug.Log("Door unlocked!");
        door.SetActive(false); // Disabling the door to simulate unlocking

        // Optionally, play sound or animation here

        // Destroy lock or make it inactive
        Destroy(gameObject);
    }
}
