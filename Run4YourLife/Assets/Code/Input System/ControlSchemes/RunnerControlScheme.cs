using Run4YourLife.Player;

namespace Run4YourLife.Input
{
    public class RunnerControlScheme : PlayerControlScheme
    {
        public Action Vertical { get; private set; }
        public Action Move { get; private set; }
        public Action Jump { get; private set; }
        public Action Dash { get; private set; }
        public Action Rock { get; private set; }

        public RunnerControlScheme()
        {
            actions.Add(Vertical = new Action(new InputSource(Axis.LEFT_VERTICAL)));
            actions.Add(Move = new Action(new InputSource(Axis.LEFT_HORIZONTAL)));
            actions.Add(Jump = new Action(new InputSource(Button.A)));
            actions.Add(Dash = new Action(new InputSource(Button.X)));
            actions.Add(Rock = new Action(new InputSource(Button.B)));
        }
    }
}