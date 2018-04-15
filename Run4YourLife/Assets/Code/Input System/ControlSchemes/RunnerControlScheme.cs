using Run4YourLife.Player;

namespace Run4YourLife.Input
{
    public class RunnerControlScheme : ControlScheme
    {
        public Action vertical;
        public Action move;
        public Action jump;
        public Action interact;
        public Action rock;

        private void Awake()
        {
            actions.Add(vertical = new Action(new InputSource(Axis.LEFT_VERTICAL)));
            actions.Add(move = new Action(new InputSource(Axis.LEFT_HORIZONTAL)));
            actions.Add(jump = new Action(new InputSource(Button.A)));
            actions.Add(interact = new Action(new InputSource(Button.X)));
            actions.Add(rock = new Action(new InputSource(Button.B)));
        }
    }
}