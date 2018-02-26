using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.Input
{
    public class PlayerStandControllerControlScheme : ControlScheme {
        public Action nextStand;
        public Action previousStand;
        public Action getBoss;
        public Action leaveGame;

        void Start()
        {
            inputDevice = GetComponent<PlayerInstance>().PlayerDefinition.inputDevice;

            actions.Add(nextStand = new Action(new InputSource(Button.RB, inputDevice)));
            actions.Add(previousStand = new Action(new InputSource(Button.LB, inputDevice)));
            actions.Add(getBoss = new Action(new InputSource(Button.X, inputDevice)));
            actions.Add(leaveGame = new Action(new InputSource(Button.B, inputDevice)));
        }
    }
}