using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public Transform destination;
    public string connect_code;

    public Vector3 position { get { return transform.position; } }

    [SerializeField] bool _isInteractable;
    public bool isInteractable 
    {
        get { return _isInteractable; }
        set { _isInteractable = value; }
    }

    public delegate void DoorEvent(object interacter, string connect_code);
    public static DoorEvent DoorEntered;

    private void OnEnable()
    {

        DoorEntered += OnEnterDoor;
    }

    private void OnDisable()
    {
        DoorEntered -= OnEnterDoor;
    }

    public void Interact(object _interacter)
    {
        DoorEntered -= OnEnterDoor;
        DoorEntered?.Invoke(_interacter, connect_code);
        DoorEntered += OnEnterDoor;
    }

    void OnEnterDoor(object interacter, string in_code)
    {
        var inter = interacter as PlayerController;

        if (in_code == connect_code)
        {
            inter.transform.position = destination.position;
        }
    }
}
