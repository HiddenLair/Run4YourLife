using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.Input
{
    public class BossControlScheme : ControlScheme
    {
        public Action MoveTrapIndicatorVertical { get; private set; }
        public Action MoveTrapIndicatorHorizontal { get; private set; }

        public Action Skill1 { get; private set; }
        public Action Skill2 { get; private set; }
        public Action Skill3 { get; private set; }
        public Action Skill4 { get; private set; }

        public Action NextSet { get; private set; }
        public Action PreviousSet { get; private set; }

        public Action Shoot { get; private set; }
        public Action Melee { get; private set; }

        public Action MoveLaserVertical { get; private set; }
        public Action MoveLaserHorizontal { get; private set; }

        public BossControlScheme()
        {
            actions.Add(MoveTrapIndicatorVertical = new Action(new InputSource(Axis.LEFT_VERTICAL)));
            actions.Add(MoveTrapIndicatorHorizontal = new Action(new InputSource(Axis.LEFT_HORIZONTAL)));

            actions.Add(Skill1 = new Action(new InputSource(Button.A)));
            actions.Add(Skill2 = new Action(new InputSource(Button.X)));
            actions.Add(Skill3 = new Action(new InputSource(Button.Y)));
            actions.Add(Skill4 = new Action(new InputSource(Button.B)));

            actions.Add(NextSet = new Action(new InputSource(Button.RB)));
            actions.Add(PreviousSet = new Action(new InputSource(Button.LB)));

            actions.Add(Shoot = new Action(new InputSource(Trigger.RIGHT)));
            actions.Add(Melee = new Action(new InputSource(Trigger.LEFT)));

            actions.Add(MoveLaserVertical = new Action(new InputSource(Axis.RIGHT_VERTICAL)));
            actions.Add(MoveLaserHorizontal = new Action(new InputSource(Axis.RIGHT_HORIZONTAL)));
        }
    }
}