using Run4YourLife.GameManagement;
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
            Actions.Add(Vertical = new Action(new InputSource(Axis.LEFT_VERTICAL), "Vertical"));
            Actions.Add(Move = new Action(new InputSource(Axis.LEFT_HORIZONTAL), "Move"));
            Actions.Add(Jump = new Action(new InputSource(Button.A), "Jump"));
            Actions.Add(Dash = new Action(new InputSource(Button.X), "Dash"));
            Actions.Add(Rock = new Action(new InputSource(Button.B), "Rock"));
        }
    }
}