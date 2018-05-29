namespace Run4YourLife.InputManagement
{
    public class RunnerStandControllerControlScheme : PlayerStandControllerControlScheme
    {
        public InputAction NextStand { get; private set; }
        public InputAction PreviousStand { get; private set; }
        public InputAction SetAsBoss { get; private set; }

        public RunnerStandControllerControlScheme()
        {
            InputActions.Add(NextStand = new InputAction(new InputSource(Button.RB), "NextStand"));
            InputActions.Add(PreviousStand = new InputAction(new InputSource(Button.LB), "PreviousStand"));
            InputActions.Add(SetAsBoss = new InputAction(new InputSource(Button.X), "SetAsBoss"));
        }
    }
}