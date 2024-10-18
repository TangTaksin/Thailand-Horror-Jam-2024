using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public Transform destination; // The position where the player will appear in the next level
    public string connect_code;   // Code to identify the door connection
    [SerializeField] public int targetLevelIndex;  // Index of the target level to load when entering the door
    [SerializeField] public float delaytransition = 1f;
    public Vector3 position { get { return transform.position; } }

    [SerializeField] public bool _isInteractable;
    public bool isInteractable
    {
        get { return _isInteractable; }
        set { _isInteractable = value; }
    }

    public delegate void DoorEvent(object interacter, string connect_code);
    public static DoorEvent DoorEntered;

    private LevelManager levelManager; // Reference to the LevelManager

    private void OnEnable()
    {
        DoorEntered += OnEnterDoor;
        levelManager = FindObjectOfType<LevelManager>(); // Find the LevelManager in the scene
    }

    private void OnDisable()
    {
        DoorEntered -= OnEnterDoor;
    }

    public void SetInteractable(bool state)
    {
        isInteractable = state;
    }

    // Interaction method
    public void Interact(object _interacter)
    {
        DoorEntered -= OnEnterDoor;
        DoorEntered?.Invoke(_interacter, connect_code); // Invoke the door event
        DoorEntered += OnEnterDoor;
    }

    // Handle the event when the player enters the door
    void OnEnterDoor(object interacter, string in_code)
    {
        var inter = interacter as PlayerController;

        // Check if the connect code matches this door's code
        if (in_code == connect_code)
        {
            // Load the target level and move the player to the destination point
            if (levelManager != null)
            {
                StartCoroutine(TransitionWithDelay(inter));

            }
        }
    }

    // Coroutine to add a delay before the transition
    private IEnumerator TransitionWithDelay(PlayerController player)
    {
        player.DisableInput();

        Transition.CalledFadeIn();

        yield return new WaitForSeconds(delaytransition);

        levelManager.LoadLevel(targetLevelIndex);

        player.transform.position = destination.position;

        player.EnableInput();
    }
}