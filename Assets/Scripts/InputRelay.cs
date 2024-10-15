using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputRelay : MonoBehaviour
{
    bool canPlayerInput = true;

    // Delegate for actions that require an InputValue parameter
    public delegate void InputEvent(InputValue value);

    // Delegate for actions that don't require an InputValue parameter
    public delegate void InputEventNoArgs();

    // Static events with appropriate delegate types
    public static InputEvent Move;
    public static InputEvent Jump;
    public static InputEventNoArgs Interact;
    public static InputEventNoArgs Confirm;

    public void OnMove(InputValue value)
    {
        Move?.Invoke(value);  // Invoke Move with the InputValue parameter
    }

    public void OnJump(InputValue value)
    {
        Jump?.Invoke(value);  // Invoke Jump with the InputValue parameter
    }

    public void OnInteract()
    {
        Interact?.Invoke();  // Invoke Interact without parameters
    }

    public void OnConfirm()
    {
        if (canPlayerInput)
        {
            Confirm?.Invoke();  // Invoke Confirm without parameters
        }
    }

    // This method handles button press input using the InputAction.CallbackContext
    public void OnConfirm(InputAction.CallbackContext context)
    {
        if (canPlayerInput && context.started)  // Check if the action has started
        {
            Confirm?.Invoke();  // Invoke Confirm without parameters
        }
    }

    public void SetPlayerCanInput(bool canInput)
    {
        canPlayerInput = canInput;
    }
}
