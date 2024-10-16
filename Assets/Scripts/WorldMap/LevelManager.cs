using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levels;     // Array of level GameObjects
    [SerializeField] private Transform[] entryPoints; // Entry points for each level
    private GameObject player;                        // Reference to the player
    private CinemachineConfiner2D cinemachineConfiner; // Reference to Cinemachine Confiner2D

    private void Start()
    {
        player = GameObject.FindWithTag("Player"); // Find the player by tag
        cinemachineConfiner = FindObjectOfType<CinemachineConfiner2D>(); // Find Cinemachine Confiner2D
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

        // Update Cinemachine confiner with the new level's bounding shape
        UpdateCinemachineConfiner(levelIndex);
    }

    // Update Cinemachine confiner with the new level's PolygonCollider2D
    private void UpdateCinemachineConfiner(int levelIndex)
    {
        PolygonCollider2D newConfiner = levels[levelIndex].GetComponentInChildren<PolygonCollider2D>();
        if (newConfiner != null && cinemachineConfiner != null)
        {
            cinemachineConfiner.m_BoundingShape2D = newConfiner; // Update the confiner
            
            // This step ensures that the new confiner is applied
            cinemachineConfiner.m_BoundingShape2D = null; // Reset the shape temporarily
            cinemachineConfiner.m_BoundingShape2D = newConfiner; // Reassign to refresh the confiner

            Debug.Log("Cinemachine confiner updated with new bounding shape.");
        }
        else
        {
            Debug.LogError("No valid PolygonCollider2D found for confiner on level " + levelIndex);
        }
    }
}
