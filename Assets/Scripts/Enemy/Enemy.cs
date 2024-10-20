using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float damage = 20f; // Damage dealt by the enemy
    public float attackRange = 1f; // Attack range for the enemy
    public LayerMask playerLayer; // Layer to detect player
    public float attackCooldown = 2f; // Cooldown between attacks (in seconds)

    private float lastAttackTime = 0f; // Time since last attack

    void Update()
    {
        // Check if enough time has passed since the last attack
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // Check for the player in the attack range
            Collider2D player = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
            if (player != null)
            {
                // If the player is in range, deal damage
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null) // Ensure player has the PlayerController component
                {
                    playerController.TakeDamage(damage);
                    lastAttackTime = Time.time; // Reset the attack timer
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw attack range in the editor for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
