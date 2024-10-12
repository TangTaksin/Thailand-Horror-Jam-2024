using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f; // Speed of player movement
    public float maxHealth = 100f; // Maximum health of the player
    public float currentHealth; // Current health of the player

    private Rigidbody2D rb; // Rigidbody component for physics
    private Vector2 movement; // Movement input vector
    private Vector3 savePoint; // Save point position
    private float saveHealth; // Save point health

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InitializePlayer();
    }

    void Update()
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

    /// <summary>
    /// Initializes the player's attributes.
    /// </summary>
    private void InitializePlayer()
    {
        currentHealth = maxHealth; // Set full health at the start
        savePoint = transform.position; // Initialize save point to the starting position
        saveHealth = maxHealth; // Initialize save health to max health
    }

    /// <summary>
    /// Handles player movement based on input.
    /// </summary>
    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Flip player sprite based on movement direction
        transform.localScale = new Vector3(moveInput > 0 ? 1 : (moveInput < 0 ? -1 : transform.localScale.x), 1, 1);
    }

    /// <summary>
    /// Handles player interactions.
    /// </summary>
    private void HandleInteractions()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Implement interaction logic here
            Debug.Log("Interacting with object");
        }
    }

    /// <summary>
    /// Handles the "Under the Legs" mechanic.
    /// </summary>
    private void HandleUnderLegs()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Implement "Under the Legs" mechanic here
            Debug.Log("Under the Legs activated");
        }
    }

    /// <summary>
    /// Simulates damage taken for testing purposes.
    /// </summary>
    private void SimulateDamage()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(50f); // Example damage
        }
    }

    /// <summary>
    /// Applies damage to the player.
    /// </summary>
    /// <param name="damage">Amount of damage taken.</param>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took damage: {damage}. Current health: {currentHealth}");
    }

    /// <summary>
    /// Handles player death logic.
    /// </summary>
    private void HandleDeath()
    {
        Debug.Log("Player died!");
        // Load player data to respawn at the last save point or current data if no save exists
        SaveSystem.LoadPlayer(gameObject);
    }

    /// <summary>
    /// Sets the player's save point and health.
    /// </summary>
    /// <param name="position">Position to set as the save point.</param>
    /// <param name="health">Health to set at the save point.</param>
    public void SetSavePoint(Vector3 position, float health)
    {
        savePoint = position;
        saveHealth = health;
    }
}
