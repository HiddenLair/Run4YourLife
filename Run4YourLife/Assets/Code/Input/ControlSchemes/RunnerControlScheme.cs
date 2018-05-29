namespace Run4YourLife.InputManagement
{
    public class RunnerControlScheme : PlayerControlScheme
    {
        public InputAction Vertical { get; private set; }
        public InputAction Move { get; private set; }
        public InputAction Jump { get; private set; }
        public InputAction Dash { get; private set; }
        public InputAction Rock { get; private set; }
        public InputAction Focus { get; private set; }

        public RunnerControlScheme()
        {
            InputActions.Add(Vertical = new InputAction(new InputSource(Axis.LEFT_VERTICAL), "Vertical"));
            InputActions.Add(Move = new InputAction(new InputSource(Axis.LEFT_HORIZONTAL), "Move"));
            InputActions.Add(Jump = new InputAction(new InputSource(Button.A), "Jump"));
            InputActions.Add(Dash = new InputAction(new InputSource(Button.X), "Dash"));
            InputActions.Add(Rock = new InputAction(new InputSource(Button.B), "Rock"));
            InputActions.Add(Focus = new InputAction(new InputSource(Button.LB), "Focus"));
        }
    }
}