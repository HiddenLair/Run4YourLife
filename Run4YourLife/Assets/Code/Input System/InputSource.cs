using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.GameInput;

public class InputSource {
    private string input;
    private InputDevice inputDevice;

    public InputSource(Axis axis, InputDevice inputDevice) : this(axis.ID, inputDevice) {}
    public InputSource(Button button, InputDevice inputDevice) : this(button.ID, inputDevice) { }

    private InputSource(string input, InputDevice inputDevice)
    {
        this.input = input;
        this.inputDevice = inputDevice;
    }

    public bool ButtonDown()
    {
        return Input.GetButtonDown(inputDevice.InputString(input));
    }

    public bool Button()
    {
        return Input.GetButton(inputDevice.InputString(input));
    }

    public bool ButtonUp()
    {
        return Input.GetButtonUp(inputDevice.InputString(input));
    }

    public float Value()
    {
        return Input.GetAxis(inputDevice.InputString(input));
    }
}
