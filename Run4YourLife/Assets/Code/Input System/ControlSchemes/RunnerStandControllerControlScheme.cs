namespace Run4YourLife.Input
{
    public class RunnerStandControllerControlScheme : PlayerStandControllerControlScheme
    {
        public Action NextStand { get; private set; }
        public Action PreviousStand { get; private set; }
        public Action SetAsBoss { get; private set; }

        public RunnerStandControllerControlScheme()
        {
            Actions.Add(NextStand = new Action(new InputSource(Button.RB), "NextStand"));
            Actions.Add(PreviousStand = new Action(new InputSource(Button.LB), "PreviousStand"));
            Actions.Add(SetAsBoss = new Action(new InputSource(Button.X), "SetAsBoss"));
        }
    }
}