namespace Run4YourLife.Input
{
    public abstract class PlayerStandControllerControlScheme : ControlScheme
    {
        public Action Ready { get; private set; }
        public Action Leave { get; private set; }
        public Action Exit { get; private set; }
        public Action Rotate { get; private set; }

        public PlayerStandControllerControlScheme()
        {
            actions.Add(Ready = new Action(new InputSource(Button.START)));
            actions.Add(Leave = new Action(new InputSource(Button.B)));
            actions.Add(Exit = new Action(new InputSource(Button.Y)));
            actions.Add(Rotate = new Action(new InputSource(Axis.LEFT_HORIZONTAL)));
        }
    }
}