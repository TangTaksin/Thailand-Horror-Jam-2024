using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of the ghost
    public float detectionRadius = 5f; // Radius to detect the player
    public Transform[] waypoints; // Waypoints for patrolling
    private int currentWaypointIndex = 0; // Current waypoint index

    void Start()
    {
        
    }

    void Update()
    {
        Move(); // Handle ghost movement
    }

    private void Move()
    {
        // Patrol logic: Move between waypoints
        if (waypoints.Length > 0)
        {
            Transform target = waypoints[currentWaypointIndex];
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            // Check if reached the target waypoint
            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Move to the next waypoint
            }
        }
    }
}
