using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxEffectMultiplier = 0.5f; // Adjust the parallax effect
    [SerializeField] private bool canLoop = true; // Toggle for background looping
    [SerializeField] private bool changeColor = true; // Toggle for color change behavior
    [SerializeField] private Color startColor = Color.white; // Initial color of the background
    [SerializeField] private Color endColor = Color.blue; // Color to transition to based on movement
    [SerializeField] private float colorChangeSpeed = 0.01f; // Speed at which the color fades

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Cache camera reference and initial position
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;

        // Cache the sprite renderer and set initial color
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = startColor;

        // Calculate texture width for looping
        textureUnitSizeX = spriteRenderer.bounds.size.x;
    }

    void FixedUpdate()
    {
        // Move background based on the camera movement and parallax effect
        float deltaX = cameraTransform.position.x - lastCameraPosition.x;
        transform.position += Vector3.right * deltaX * parallaxEffectMultiplier;

        lastCameraPosition = cameraTransform.position;

        // Perform background looping if enabled
        if (canLoop)
        {
            LoopBackground();
        }

        // Apply color change based on camera movement if changeColor is enabled
        if (changeColor)
        {
            UpdateBackgroundColor(deltaX);
        }
    }

    private void LoopBackground()
    {
        float cameraToBackgroundDistance = cameraTransform.position.x - transform.position.x;

        if (Mathf.Abs(cameraToBackgroundDistance) >= textureUnitSizeX)
        {
            float offsetPositionX = cameraToBackgroundDistance % textureUnitSizeX;
            transform.position += Vector3.right * offsetPositionX;
        }
    }

    private void UpdateBackgroundColor(float deltaX)
    {
        // Calculate a value based on how much the camera has moved
        float t = Mathf.Abs(deltaX * colorChangeSpeed);

        // Linearly interpolate between the start and end colors based on movement
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, deltaX > 0 ? endColor : startColor, t);
    }
}
