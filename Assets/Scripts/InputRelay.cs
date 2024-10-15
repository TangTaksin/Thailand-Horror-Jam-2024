using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputRelay : MonoBehaviour
{
    public delegate void InputEvent(InputValue value);
    public static InputEvent Move;
    public static InputEvent Jump;
    public static InputEvent Interact;
    public static InputEvent Confirm; 

    public void OnMove(InputValue value)
    {
        Move?.Invoke(value);
    }

    public void OnJump(InputValue value)
    {
        Jump?.Invoke(value);
    }

    public void OnInteract(InputValue value)
    {
        Interact?.Invoke(value);
    }

    public void OnConfirm(InputValue value)
    {
        Confirm?.Invoke(value);
    }
}
