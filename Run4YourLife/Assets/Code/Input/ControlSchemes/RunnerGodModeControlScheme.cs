using Run4YourLife.GameManagement;

namespace Run4YourLife.InputManagement
{
    public class RunnerGodModeControlScheme : PlayerControlScheme
    {
        public InputAction MoveH { get; private set; }
        public InputAction MoveV { get; private set; }
        public InputAction MSpeed { get; private set; }
        public InputAction End { get; private set; }

        public RunnerGodModeControlScheme()
        {
            InputActions.Add(MoveV = new InputAction(new InputSource(Axis.LEFT_VERTICAL), "MoveV"));
            InputActions.Add(MoveH = new InputAction(new InputSource(Axis.LEFT_HORIZONTAL), "MoveH"));
            InputActions.Add(MSpeed = new InputAction(new InputSource(Button.A), "MSpeed"));
            InputActions.Add(End = new InputAction(new InputSource(Button.B), "End"));
        }
    }
}