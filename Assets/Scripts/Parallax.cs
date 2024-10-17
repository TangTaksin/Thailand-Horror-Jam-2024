using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float baseParallaxEffectMultiplier = 0.1f; // Base multiplier for parallax effect
    public bool canLoop = true; // Variable to toggle looping on/off
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    [SerializeField] private float textureUnitSizeX;

    // Parallax smoothing speed
    public float smoothFactor = 5f; // Adjust this value for smoothness

    void Start()
    {
        cameraTransform = Camera.main.transform; // Get the main camera's transform
        lastCameraPosition = cameraTransform.position; // Store the initial position of the camera

        // Calculate the size of the texture to loop
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        textureUnitSizeX = spriteRenderer.bounds.size.x; // Get the width of the sprite
    }

    void FixedUpdate()
    {
        // Calculate how much the camera has moved
        float deltaX = cameraTransform.position.x - lastCameraPosition.x;

        // Calculate camera speed
        float cameraSpeed = Mathf.Abs(deltaX); // Get the speed of the camera movement

        // Calculate dynamic parallax multiplier based on camera speed
        float dynamicParallaxEffectMultiplier = baseParallaxEffectMultiplier * (cameraSpeed / 10f); // Adjust 10f as needed

        // Calculate target position using parallax effect
        Vector3 targetPosition = transform.position + new Vector3(deltaX * dynamicParallaxEffectMultiplier, 0, 0);

        // Smoothly move the background layer towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);

        // Update the last camera position
        lastCameraPosition = cameraTransform.position;

        // Only perform background looping if canLoop is true
        if (canLoop)
        {
            // Loop the background
            if (cameraTransform.position.x >= transform.position.x + textureUnitSizeX)
            {
                transform.position += new Vector3(textureUnitSizeX, 0, 0);
            }
            else if (cameraTransform.position.x <= transform.position.x - textureUnitSizeX)
            {
                transform.position -= new Vector3(textureUnitSizeX, 0, 0);
            }
        }
    }
}
