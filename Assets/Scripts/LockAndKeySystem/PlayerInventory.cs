using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    // List of keys the player has collected
    private List<string> collectedKeys = new List<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to add key to inventory
    public void AddKey(string key)
    {
        if (!collectedKeys.Contains(key))
        {
            collectedKeys.Add(key);
            Debug.Log(key + " added to inventory.");
        }
    }

    // Method to check if the player has the required key
    public bool HasKey(string key)
    {
        return collectedKeys.Contains(key);
    }
}
