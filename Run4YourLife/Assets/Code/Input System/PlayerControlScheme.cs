using Run4YourLife.Player;

namespace Run4YourLife.Input
{
    public class PlayerControlScheme : ControlScheme
    {
        public Action move;
        public Action jump;
        public Action interact;

        private void Awake()
        {
            actions.Add(move = new Action(new InputSource(Axis.LEFT_HORIZONTAL)));
            actions.Add(jump = new Action(new InputSource(Button.A)));
            actions.Add(interact = new Action(new InputSource(Button.X)));
        }

        private void Start()
        {
            InitializeActionsWithPlayerInputDevice();
        }
    }
}