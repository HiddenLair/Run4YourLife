﻿namespace Run4YourLife.Input
{
    public class BossStandControllerControlScheme : PlayerStandControllerControlScheme
    {
        public Action SetAsRunner { get; private set; }

        public BossStandControllerControlScheme()
        {
            actions.Add(SetAsRunner = new Action(new InputSource(Button.X)));
        }
    }
}