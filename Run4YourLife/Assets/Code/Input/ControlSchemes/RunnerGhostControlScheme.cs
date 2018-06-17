namespace Run4YourLife.InputManagement
{
    public class RunnerGhostControlScheme : PlayerControlScheme
    {
        public InputAction Vertical { get; private set; }
        public InputAction Horizontal { get; private set; }

        public RunnerGhostControlScheme()
        {
            InputActions.Add(Vertical = new InputAction(new InputSource(Axis.LEFT_VERTICAL), "Vertical"));
            InputActions.Add(Horizontal = new InputAction(new InputSource(Axis.LEFT_HORIZONTAL), "Horizontal"));
        }
    }
}