using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxEffectMultiplier; // Adjust this value for the desired parallax effect
    public bool canLoop = true; // Variable to toggle looping on/off
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    [SerializeField] private float textureUnitSizeX;

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

        // Move the background layer with parallax effect
        transform.position += new Vector3(deltaX * parallaxEffectMultiplier, 0, 0);

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
