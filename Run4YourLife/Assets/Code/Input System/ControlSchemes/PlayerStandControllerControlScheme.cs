namespace Run4YourLife.Input
{
    public abstract class PlayerStandControllerControlScheme : ControlScheme
    {
        public Action ready;
        public Action leave;
        public Action exit;
        public Action rotate;

        public PlayerStandControllerControlScheme()
        {
            actions.Add(ready = new Action(new InputSource(Button.START)));
            actions.Add(leave = new Action(new InputSource(Button.B)));
            actions.Add(exit = new Action(new InputSource(Button.Y)));
            actions.Add(rotate = new Action(new InputSource(Axis.LEFT_HORIZONTAL)));
        }
    }
}