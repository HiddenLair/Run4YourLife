using Run4YourLife.Player;

namespace Run4YourLife.Input
{
    public class PlayerControlScheme : ControlScheme
    {
        public Action move;
        public Action jump;
        public Action interact;

        void Start()
        {
            inputDevice = GetComponent<PlayerInstance>().PlayerDefinition.inputDevice;

            actions.Add(move = new Action(new InputSource(Axis.LEFT_HORIZONTAL, inputDevice)));
            actions.Add(jump = new Action(new InputSource(Button.A, inputDevice)));
            actions.Add(interact = new Action(new InputSource(Button.X, inputDevice)));
        }
    }
}