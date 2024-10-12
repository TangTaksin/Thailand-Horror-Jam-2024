using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxHealth = 100f;
    public float currentHealth;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector3 savePoint; // Variable to hold the save point position
    private float saveHealth;   // Variable to hold the save point health

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth; // Set player to full health at the start
        savePoint = transform.position; // Initialize save point to the starting position
        saveHealth = maxHealth; // Initialize save health to max health
    }

    void Update()
    {
        // Movement input (only horizontal)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = 0; // Ensure vertical movement is always zero

        // Simulate taking damage for testing (hit the player with space bar)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(50f); // Example damage
        }

        // Check if player is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        // Move the player horizontally
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took damage: {damage}. Current health: {currentHealth}");
    }

    void Die()
    {
        Debug.Log("Player died!");
        // Load player data to respawn at the last save point or current data if no save exists
        SaveSystem.LoadPlayer(gameObject);
    }

    // Set the player's save point and health
    public void SetSavePoint(Vector3 position, float health)
    {
        savePoint = position;
        saveHealth = health;
    }
}
