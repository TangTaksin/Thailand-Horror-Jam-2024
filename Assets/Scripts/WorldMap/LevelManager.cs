using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levels;     // Array of level GameObjects
    [SerializeField] private Transform[] entryPoints; // Entry points for each level
    private GameObject player;                        // Reference to the player

    private void Start()
    {
        player = GameObject.FindWithTag("Player"); // Find the player by tag
        LoadLevel(0);                              // Start at level 0
    }

    // Load the specified level and move the player to its entry point
    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Length)
        {
            Debug.LogError("Invalid level index.");
            return;
        }

        // Deactivate all levels except the specified one
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetActive(i == levelIndex);
        }

        // Move the player to the entry point for the new level
        player.transform.position = entryPoints[levelIndex].position;
    }
}
