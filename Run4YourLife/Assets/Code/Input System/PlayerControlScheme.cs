using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.GameInput;

public class PlayerControlScheme : ControlScheme {
    public Action move;
    public Action jump;
    public Action interact;

    void Awake()
    {
        inputDevice = new InputDevice(1); 

        move = new Action(new InputSource(Axis.LEFT_HORIZONTAL,inputDevice));
        jump = new Action(new InputSource(Button.A, inputDevice));
        interact = new Action(new InputSource(Button.X, inputDevice));

        actions.Add(move);
        actions.Add(jump);
        actions.Add(interact);
    }
}