using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.GameManagement;

namespace Run4YourLife.InputManagement
{
    public class BossControlScheme : PlayerControlScheme
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
            Actions.Add(MoveTrapIndicatorVertical = new Action(new InputSource(Axis.LEFT_VERTICAL), "MoveTrapIndicatorVertical"));
            Actions.Add(MoveTrapIndicatorHorizontal = new Action(new InputSource(Axis.LEFT_HORIZONTAL), "MoveTrapIndicatorHorizontal"));

            Actions.Add(Skill1 = new Action(new InputSource(Button.A), "Skill1"));
            Actions.Add(Skill2 = new Action(new InputSource(Button.X), "Skill2"));
            Actions.Add(Skill3 = new Action(new InputSource(Button.Y), "Skill3"));
            Actions.Add(Skill4 = new Action(new InputSource(Button.B), "Skill4"));

            Actions.Add(NextSet = new Action(new InputSource(Button.RB), "NextSet"));
            Actions.Add(PreviousSet = new Action(new InputSource(Button.LB), "PreviousSet"));

            Actions.Add(Shoot = new Action(new InputSource(Trigger.RIGHT), "Shoot"));
            Actions.Add(Melee = new Action(new InputSource(Trigger.LEFT), "Melee"));

            Actions.Add(MoveLaserVertical = new Action(new InputSource(Axis.RIGHT_VERTICAL), "MoveLaserVertical"));
            Actions.Add(MoveLaserHorizontal = new Action(new InputSource(Axis.RIGHT_HORIZONTAL), "MoveLaserHorizontal"));
        }
    }
}