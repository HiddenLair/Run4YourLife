namespace Run4YourLife.InputManagement
{
    public abstract class PlayerStandControllerControlScheme : ControlScheme
    {
        public InputAction Ready { get; private set; }
        public InputAction Rotate { get; private set; }

        public PlayerStandControllerControlScheme()
        {
            InputActions.Add(Ready = new InputAction(new InputSource(Button.START), "Ready"));
            InputActions.Add(Rotate = new InputAction(new InputSource(Axis.LEFT_HORIZONTAL), "Rotate"));
        }
    }
}