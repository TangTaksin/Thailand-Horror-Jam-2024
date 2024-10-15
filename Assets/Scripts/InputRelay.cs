using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputRelay : MonoBehaviour
{
    bool canPlayerInput = true;
    void Start()
    {

    }
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

    }

    // // Call this method on input from your Input Actions
    // public void OnInteract()
    // {
    //     Interact?.Invoke();
    // }

    // // Call this method on input from your Input Actions
    // public void OnConfirm()
    // {
    //     if (canPlayerInput)
    //     {
    //         Confirm?.Invoke();
    //     }

    // }

    // // This can be connected to an Input Action for button press
    // public void OnConfirm(InputAction.CallbackContext context)
    // {
    //     if (canPlayerInput)
    //     {
    //         if (context.started) // Check if the action has just started
    //         {
    //             Confirm?.Invoke(); // Notify listeners
    //         }

    //     }

    // }

    public void SetPlayerCanInput(bool canInput)
    {
        canPlayerInput = canInput;
    }
}
