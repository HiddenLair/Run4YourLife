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
            actions.Add(moveTrapIndicatorVertical = new Action(new InputSource(Axis.LEFT_VERTICAL)));
            actions.Add(moveTrapIndicatorHorizontal = new Action(new InputSource(Axis.LEFT_HORIZONTAL)));

            actions.Add(skill1 = new Action(new InputSource(Button.A)));
            actions.Add(skill2 = new Action(new InputSource(Button.X)));
            actions.Add(skill3 = new Action(new InputSource(Button.Y)));
            actions.Add(skill4 = new Action(new InputSource(Button.B)));

            actions.Add(nextSet = new Action(new InputSource(Button.RB)));
            actions.Add(previousSet = new Action(new InputSource(Button.LB)));

            actions.Add(shoot = new Action(new InputSource(Trigger.RIGHT)));
            actions.Add(melee = new Action(new InputSource(Trigger.LEFT)));

            actions.Add(moveLaserVertical = new Action(new InputSource(Axis.RIGHT_VERTICAL)));
        }
    }
}