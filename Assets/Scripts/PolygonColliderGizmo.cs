using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonColliderGizmo : MonoBehaviour
{
    private PolygonCollider2D polygonCollider;

    private void OnDrawGizmos()
    {
        // Fetch the PolygonCollider2D attached to this GameObject
        polygonCollider = GetComponent<PolygonCollider2D>();

        // Set the Gizmos color
        Gizmos.color = Color.green;

        // Loop through each path in the polygon collider (in case it has multiple paths)
        for (int i = 0; i < polygonCollider.pathCount; i++)
        {
            Vector2[] points = polygonCollider.GetPath(i);

            // Draw the outline of the polygon
            for (int j = 0; j < points.Length; j++)
            {
                // Get the start and end points of the line segment
                Vector2 pointA = points[j];
                Vector2 pointB = points[(j + 1) % points.Length]; // Wrap back to first point

                // Transform points to world space (since the collider points are in local space)
                Vector2 worldPointA = transform.TransformPoint(pointA);
                Vector2 worldPointB = transform.TransformPoint(pointB);

                // Draw a line between these two points
                Gizmos.DrawLine(worldPointA, worldPointB);
            }
        }
    }
}
