using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.GameManagement;

namespace Run4YourLife.InputManagement
{
    public class BossControlScheme : PlayerControlScheme
    {
        public InputAction MoveTrapIndicatorVertical { get; private set; }
        public InputAction MoveTrapIndicatorHorizontal { get; private set; }

        public InputAction Skill1 { get; private set; }
        public InputAction Skill2 { get; private set; }
        public InputAction Skill3 { get; private set; }
        public InputAction Skill4 { get; private set; }

        public InputAction NextSet { get; private set; }
        public InputAction PreviousSet { get; private set; }

        public InputAction Shoot { get; private set; }
        public InputAction Melee { get; private set; }

        public InputAction MoveLaserVertical { get; private set; }
        public InputAction MoveLaserHorizontal { get; private set; }

        public BossControlScheme()
        {
            InputActions.Add(MoveTrapIndicatorVertical = new InputAction(new InputSource(Axis.LEFT_VERTICAL), "MoveTrapIndicatorVertical"));
            InputActions.Add(MoveTrapIndicatorHorizontal = new InputAction(new InputSource(Axis.LEFT_HORIZONTAL), "MoveTrapIndicatorHorizontal"));

            InputActions.Add(Skill1 = new InputAction(new InputSource(Button.A), "Skill1"));
            InputActions.Add(Skill2 = new InputAction(new InputSource(Button.X), "Skill2"));
            InputActions.Add(Skill3 = new InputAction(new InputSource(Button.Y), "Skill3"));
            InputActions.Add(Skill4 = new InputAction(new InputSource(Button.B), "Skill4"));

            InputActions.Add(NextSet = new InputAction(new InputSource(Button.RB), "NextSet"));
            InputActions.Add(PreviousSet = new InputAction(new InputSource(Button.LB), "PreviousSet"));

            InputActions.Add(Shoot = new InputAction(new InputSource(Trigger.RIGHT), "Shoot"));
            InputActions.Add(Melee = new InputAction(new InputSource(Trigger.LEFT), "Melee"));

            InputActions.Add(MoveLaserVertical = new InputAction(new InputSource(Axis.RIGHT_VERTICAL), "MoveLaserVertical"));
            InputActions.Add(MoveLaserHorizontal = new InputAction(new InputSource(Axis.RIGHT_HORIZONTAL), "MoveLaserHorizontal"));
        }
    }
}