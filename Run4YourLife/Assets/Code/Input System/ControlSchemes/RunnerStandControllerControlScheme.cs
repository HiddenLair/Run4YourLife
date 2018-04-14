namespace Run4YourLife.Input
{
    public class RunnerStandControllerControlScheme : PlayerStandControllerControlScheme
    {
        public Action nextStand;
        public Action previousStand;
        public Action setAsBoss;

        protected override void OnAwake()
        {
            actions.Add(nextStand = new Action(new InputSource(Button.RB)));
            actions.Add(previousStand = new Action(new InputSource(Button.LB)));
            actions.Add(setAsBoss = new Action(new InputSource(Button.X)));
        }
    }
}