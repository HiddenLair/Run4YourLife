using Run4YourLife.GameManagement;

namespace Run4YourLife.InputManagement
{
    public abstract class PlayerControlScheme : ControlScheme
    {
        public Action Pause { get; private set; }

        public PlayerControlScheme()
        {
            Actions.Add(Pause = new Action(new InputSource(Button.START), "Pause"));
        }
    }
}
