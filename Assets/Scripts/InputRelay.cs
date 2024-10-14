using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputRelay : MonoBehaviour
{
    public delegate void InputEvent(InputValue value);
    public static InputEvent Interact;
    public static InputEvent Confirm; 

    public void OnInteract(InputValue value)
    {
        Interact?.Invoke(value);
    }

    public void OnConfirm(InputValue value)
    {
        Confirm?.Invoke(value);
    }    
}
