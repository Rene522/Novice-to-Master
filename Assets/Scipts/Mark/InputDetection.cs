using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;


[RequireComponent(typeof(PlayerInput))]
public class InputDetection : MonoBehaviour
{
    PlayerInput playerInput;
    bool previousAutoSwitchSetting;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        previousAutoSwitchSetting = playerInput.neverAutoSwitchControlSchemes;
        playerInput.neverAutoSwitchControlSchemes = true;

        InputSystem.onEvent += DetectInput;
    }

    private void OnDisable()
    {
        InputSystem.onEvent -= DetectInput;

        if (playerInput)
        {
            playerInput.neverAutoSwitchControlSchemes = previousAutoSwitchSetting;
        }
    }

    void DetectInput(InputEventPtr inputEvent, InputDevice device)
    {
        if (!playerInput || !playerInput.isActiveAndEnabled)
        {
            return;
        }

        if (!inputEvent.IsA<StateEvent>() && !inputEvent.IsA<DeltaStateEvent>())
        {
            return;
        }

        string scheme;

        if (device is Keyboard)
        {
            scheme = "KeyboardMouse";
        }
        else if (device is Gamepad)
        {
            scheme = "Gamepad";
        }
        else
        {
            return;
        }

        if (playerInput.currentControlScheme == scheme && PlayerInput.FindFirstPairedToDevice(device) == playerInput)
        {
            return;
        }

        foreach (InputControl control in inputEvent.EnumerateChangedControls(device))
        {
            if (!(control is ButtonControl button) || button.isPressed || button.ReadValueFromEvent(inputEvent) < button.pressPointOrDefault)
            {
                continue;
            }

            if (device is Keyboard && Mouse.current != null)
            {
                playerInput.SwitchCurrentControlScheme(scheme, device, Mouse.current);
            }
            else
            {
                playerInput.SwitchCurrentControlScheme(scheme, device);
            }

            return;
        }
    }
}
