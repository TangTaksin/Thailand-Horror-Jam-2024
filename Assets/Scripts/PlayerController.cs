using System.Collections;
using System.Collections.Generic;
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

    public bool isHiding = false; // Change to public to allow access from other scripts
    private HidingSpot currentHidingSpot; // Reference to the current hiding spot
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    private int normalSortingOrder; // Variable to store the normal sorting order
    private bool isUnderLegsMode = false; // Tracks whether Under the Legs mode is active

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        normalSortingOrder = spriteRenderer.sortingOrder; // Store the initial sorting order
        InitializePlayer();
    }

    void Update()
    {
        HandleMovement();
        SimulateDamage();
        HandleHiding(); // Call the hiding method
        HandleUnderLegsMode(); // Handle the Under the Legs mechanic

        if (currentHealth <= 0)
        {
            HandleDeath();
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
        if (isHiding) // Prevent movement while hiding
        {
            rb.velocity = Vector2.zero; // Stop player movement when hiding
            return;
        }

        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Flip player sprite based on movement direction
        transform.localScale = new Vector3(moveInput > 0 ? 1 : (moveInput < 0 ? -1 : transform.localScale.x), 1, 1);
    }

    private void HandleHiding()
    {
        if (currentHidingSpot != null) // Only allow hiding if in a hiding spot
        {
            if (Input.GetKeyDown(KeyCode.S)) // Check if S key is pressed
            {
                isHiding = true; // Set hiding state
                SetAlpha(0.5f); // Set transparency to 50%
                spriteRenderer.sortingOrder = 2; // Change to the desired sorting order when hiding
                Debug.Log("Player is hiding in a hiding spot."); // Optional: log to console
            }

            if (Input.GetKeyUp(KeyCode.S)) // Check if S key is released
            {
                isHiding = false; // Exit hiding state
                SetAlpha(1f);
                spriteRenderer.sortingOrder = normalSortingOrder;
                Debug.Log("Player is no longer hiding.");
            }
        }
    }

    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color; 
        color.a = alpha; 
        spriteRenderer.color = color; 
    }

    private void SimulateDamage()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(50f);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isHiding)
        {
            currentHealth -= damage;
        }
    }

    private void HandleDeath()
    {
        SaveSystem.LoadPlayer(gameObject);
    }

    public void SetSavePoint(Vector3 position, float health)
    {
        savePoint = position;
        saveHealth = health;
    }

    public void EnterHidingSpot(HidingSpot hidingSpot)
    {
        currentHidingSpot = hidingSpot;
        Debug.Log("Entered hiding spot.");
    }

    public void ExitHidingSpot(HidingSpot hidingSpot)
    {
        if (currentHidingSpot == hidingSpot)
        {
            currentHidingSpot = null;
            Debug.Log("Exited hiding spot.");
        }
    }

    private void HandleUnderLegsMode()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isHiding) 
        {
            isUnderLegsMode = true;
            ToggleVisibility(true);
            Debug.Log("Under the Legs mode activated.");
        }
    }

    private void ToggleVisibility(bool underLegsMode)
    {
        foreach (var enemy in FindObjectsOfType<Ghost>())
        {
            enemy.ToggleVisibility(underLegsMode);
        }
    }

    public bool IsHiding()
    {
        return isHiding;
    }
}
