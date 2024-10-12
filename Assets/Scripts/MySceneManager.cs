using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Required for TextMesh Pro
using UnityEngine.SceneManagement; // Required for scene management

public class MySceneManager : MonoBehaviour
{
    public string sceneToLoad = "YourSceneName"; // Replace with your target scene name
    public TextMeshProUGUI statusText; // Reference to the TMP text component

    private bool isPlayerInTrigger = false; // To check if player is in the trigger

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the status text is hidden at the start
        HideStatusText(); // Call to hide status text initially
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is in trigger and "E" is pressed
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            ChangeSceneByName();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger
        if (other.CompareTag("Player")) // Make sure the player has the "Player" tag
        {
            isPlayerInTrigger = true;
            UpdateStatusText("Press 'E' to change the scene."); // Use UpdateStatusText
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exits the trigger
        if (other.CompareTag("Player")) // Make sure the player has the "Player" tag
        {
            isPlayerInTrigger = false;
            HideStatusText(); // Call to hide status text
        }
    }

    private void UpdateStatusText(string message)
    {
        // Update the status text and its color
        if (statusText != null)
        {
            statusText.text = message; // Set the status text to the provided message
        }
    }

    private void HideStatusText()
    {
        // Clear the status text in the TMP text
        if (statusText != null)
        {
            statusText.text = ""; // Clear the message
        }
    }

    private void ChangeSceneByName()
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneToLoad); // Change to your target scene name
    }
}
