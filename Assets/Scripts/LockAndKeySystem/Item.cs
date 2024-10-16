using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject objectToDestroy; // Object to destroy after collecting the item
    public GameObject objectToActivate; // Object to activate after collecting the item

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the collider is the Player
        {
            // If there's a referenced object to destroy, destroy it
            if (objectToDestroy != null)
            {
                Destroy(objectToDestroy);
            }

            // If there's a referenced object to activate, activate it
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }
        }
    }
}
