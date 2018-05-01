﻿using Run4YourLife.GameManagement;

namespace Run4YourLife.Input
{
    public abstract class PlayerControlScheme : ControlScheme
    {
        public Action Pause { get; private set; }

        public PlayerControlScheme()
        {
            Actions.Add(Pause = new Action(new InputSource(Button.START), "Pause"));
        }
    }
}