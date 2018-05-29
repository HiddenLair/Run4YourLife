using Run4YourLife.GameManagement;

namespace Run4YourLife.InputManagement
{
    public abstract class PlayerControlScheme : ControlScheme
    {
        public InputAction Pause { get; private set; }

        public PlayerControlScheme()
        {
            InputActions.Add(Pause = new InputAction(new InputSource(Button.START), "Pause"));
        }
    }
}
