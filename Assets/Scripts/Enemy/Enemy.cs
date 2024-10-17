using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float damage = 20f; // Damage dealt by the enemy
    public float attackRange = 1f; // Attack range for the enemy
    public LayerMask playerLayer; // Layer to detect player

    void Update()
    {
        // Check for the player in the attack range
        Collider2D player = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        if (player != null)
        {
            // If the player is in range, deal damage
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw attack range in the editor for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
