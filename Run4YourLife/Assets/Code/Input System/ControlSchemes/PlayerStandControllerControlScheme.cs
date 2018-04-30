namespace Run4YourLife.Input
{
    public abstract class PlayerStandControllerControlScheme : ControlScheme
    {
        public Action Ready { get; private set; }
        public Action Exit { get; private set; }
        public Action Rotate { get; private set; }

        public PlayerStandControllerControlScheme()
        {
            Actions.Add(Ready = new Action(new InputSource(Button.START), "Ready"));
            Actions.Add(Exit = new Action(new InputSource(Button.B), "Exit"));
            Actions.Add(Rotate = new Action(new InputSource(Axis.LEFT_HORIZONTAL), "Rotate"));
        }
    }
}