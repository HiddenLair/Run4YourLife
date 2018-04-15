namespace Run4YourLife.Input
{
    public class RunnerStandControllerControlScheme : PlayerStandControllerControlScheme
    {
        public Action NextStand { get; private set; }
        public Action PreviousStand { get; private set; }
        public Action SetAsBoss { get; private set; }

        public RunnerStandControllerControlScheme()
        {
            actions.Add(NextStand = new Action(new InputSource(Button.RB)));
            actions.Add(PreviousStand = new Action(new InputSource(Button.LB)));
            actions.Add(SetAsBoss = new Action(new InputSource(Button.X)));
        }
    }
}