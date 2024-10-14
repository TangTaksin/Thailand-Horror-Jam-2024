using UnityEngine;
using TMPro; // Include the TextMeshPro namespace

public class SavePoint : MonoBehaviour
{
    private bool playerInRange = false;

    // Reference to the TextMeshPro component
    public TextMeshProUGUI statusText;

    // Reference to the SpriteRenderer component
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Color to change the sprite to when saved
    public Color savedColor = Color.green; // Change this to your desired color

    // Color to revert to when entering the save point
    public Color defaultColor = Color.white; // Change this to your desired default color

    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Ensure the sprite is set to the default color on start
        if (spriteRenderer != null)
        {
            spriteRenderer.color = defaultColor;
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))  // Press 'E' to save
        {
            SaveSystem.SavePlayer(GameObject.FindWithTag("Player"));
            Debug.Log("Game saved at the save point.");
            UpdateStatusText("Game saved!", savedColor); // Update the status text and change color
            ChangeSpriteColor(savedColor); // Change sprite color to indicate save
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered save point range.");
            UpdateStatusText("Press 'E' to save.", Color.white); // Notify player to save
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left save point range.");
            UpdateStatusText("", Color.white); // Clear status text, set to default color
        }
    }

    // Method to update the status text
    private void UpdateStatusText(string message, Color color)
    {
        if (statusText != null)
        {
            statusText.text = message; // Set the status text to the provided message
            statusText.color = color; // Change the text color
        }
    }

    // Method to change the sprite color
    private void ChangeSpriteColor(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color; // Change the sprite color
        }
    }
}
