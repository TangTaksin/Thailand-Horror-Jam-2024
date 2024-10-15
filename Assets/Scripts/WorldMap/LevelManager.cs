using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levels; // Array to hold all level GameObjects
    private int currentLevelIndex = 0;

    void Start()
    {
        ShowLevel(currentLevelIndex);
    }

    public void ShowLevel(int levelIndex)
    {
        // Deactivate all levels
        foreach (var level in levels)
        {
            level.SetActive(false);
        }

        // Activate the current level
        if (levelIndex >= 0 && levelIndex < levels.Length)
        {
            levels[levelIndex].SetActive(true);
        }
    }

    public void NextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Length)
        {
            currentLevelIndex = 0; // Loop back to the first level
        }
        ShowLevel(currentLevelIndex);
    }
}
