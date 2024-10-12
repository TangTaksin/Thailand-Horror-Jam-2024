using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public float maxHealth = 100f;
    public float currentHealth;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector3 savePoint;
    private float saveHealth;

    // Hiding Mechanic Variables
    private bool isHiding = false; // Track if the player is currently hiding
    private HidingSpot currentHidingSpot; // Reference to the current hiding spot

    // Optionally, use a renderer to change visibility
    private SpriteRenderer spriteRenderer; // Reference to the sprite renderer

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer
        InitializePlayer();
    }

    void Update()
    {
        // Only allow movement and interactions if the player is not hiding
        if (!isHiding)
        {
            HandleMovement();
            HandleInteractions();
            HandleUnderLegs();
            SimulateDamage();

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }
        else
        {
            // Allow the player to exit hiding by pressing E
            if (Input.GetKeyDown(KeyCode.E))
            {
                ExitHidingSpot(); // Exit hiding if E is pressed
            }
        }
    }

    private void InitializePlayer()
    {
        currentHealth = maxHealth; // Set full health at the start
        savePoint = transform.position; // Initialize save point to the starting position
        saveHealth = maxHealth; // Initialize save health to max health
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Flip player sprite based on movement direction
        transform.localScale = new Vector3(moveInput > 0 ? 1 : (moveInput < 0 ? -1 : transform.localScale.x), 1, 1);
    }

    private void HandleInteractions()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentHidingSpot != null) // Check if we are near a hiding spot
            {
                EnterHidingSpot(currentHidingSpot); // Enter hiding spot
            }
            else
            {
                // Implement other interaction logic here if not hiding
                Debug.Log("Interacting with object");
            }
        }
    }

    private void HandleUnderLegs()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Implement "Under the Legs" mechanic here
            Debug.Log("Under the Legs activated");
        }
    }

    private void SimulateDamage()
    {
        // Only allow damage if the player is not hiding
        if (Input.GetKeyDown(KeyCode.K) && !isHiding)
        {
            TakeDamage(50f); // Example damage
        }
    }

    public void EnterHidingSpot(HidingSpot hidingSpot)
    {
        isHiding = true; // Set hiding state to true
        currentHidingSpot = hidingSpot; // Store the current hiding spot
        rb.velocity = Vector2.zero; // Stop player movement

        // Optionally change the player's appearance to indicate hiding
        spriteRenderer.color = new Color(1, 1, 1, 0.5f); // Make the player semi-transparent
        Debug.Log(hidingSpot.hidingMessage); // Display hiding message
    }

    public void ExitHidingSpot()
    {
        isHiding = false; // Set hiding state to false
        spriteRenderer.color = Color.white; // Restore player's appearance
        Debug.Log("Exited hiding spot");
        currentHidingSpot = null; // Clear the current hiding spot
    }

    public void SetCurrentHidingSpot(HidingSpot hidingSpot)
    {
        currentHidingSpot = hidingSpot; // Store the current hiding spot
    }

    public void ClearCurrentHidingSpot()
    {
        currentHidingSpot = null; // Clear the current hiding spot
    }

    public void TakeDamage(float damage)
    {
        if (!isHiding) // Only take damage if not hiding
        {
            currentHealth -= damage;
            Debug.Log($"Player took damage: {damage}. Current health: {currentHealth}");
        }
    }

    private void HandleDeath()
    {
        Debug.Log("Player died!");
        // Load player data to respawn at the last save point or current data if no save exists
        SaveSystem.LoadPlayer(gameObject);
    }

    public void SetSavePoint(Vector3 position, float health)
    {
        savePoint = position;
        saveHealth = health;
    }
}
