using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.Input
{
    public class BossControlScheme : ControlScheme
    {
        public Action moveTrapIndicatorVertical;
        public Action moveTrapIndicatorHorizontal;

        public Action skill1;
        public Action skill2;
        public Action skill3;
        public Action skill4;

        public Action nextSet;
        public Action previousSet;

        public Action shoot;
        public Action melee;

        public Action moveLaserVertical;


        private void Awake()
        {
            inputDevice = GetComponent<PlayerInstance>().PlayerDefinition.inputDevice;

            actions.Add(moveTrapIndicatorVertical = new Action(new InputSource(Axis.LEFT_VERTICAL, inputDevice)));
            actions.Add(moveTrapIndicatorHorizontal = new Action(new InputSource(Axis.LEFT_HORIZONTAL, inputDevice)));

            actions.Add(skill1 = new Action(new InputSource(Button.A, inputDevice)));
            actions.Add(skill2 = new Action(new InputSource(Button.X, inputDevice)));
            actions.Add(skill3 = new Action(new InputSource(Button.Y, inputDevice)));
            actions.Add(skill4 = new Action(new InputSource(Button.B, inputDevice)));

            actions.Add(nextSet = new Action(new InputSource(Button.RB, inputDevice)));
            actions.Add(previousSet = new Action(new InputSource(Button.LB, inputDevice)));

            actions.Add(shoot = new Action(new InputSource(Trigger.RIGHT, inputDevice)));
            actions.Add(melee = new Action(new InputSource(Trigger.LEFT, inputDevice)));

            actions.Add(moveLaserVertical = new Action(new InputSource(Axis.RIGHT_VERTICAL, inputDevice)));
        }

        private void Start()
        {
            InitializeActionsWithPlayerInputDevice();
        }
    }
}