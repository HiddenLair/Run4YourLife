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
        public Action startGame;
        public Action mainMenu;

        private void Awake()
        {
            actions.Add(nextStand = new Action(new InputSource(Button.RB)));
            actions.Add(previousStand = new Action(new InputSource(Button.LB)));
            actions.Add(getBoss = new Action(new InputSource(Button.X)));
            actions.Add(leaveGame = new Action(new InputSource(Button.B)));
            actions.Add(startGame = new Action(new InputSource(Button.START)));
            actions.Add(mainMenu = new Action(new InputSource(Button.Y)));
        }
    }
}