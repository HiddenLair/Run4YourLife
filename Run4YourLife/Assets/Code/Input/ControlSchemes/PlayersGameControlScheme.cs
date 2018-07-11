namespace Run4YourLife.InputManagement
{
    public class PlayersGameControlScheme : ControlScheme
    {
        public InputAction Pause { get; private set; }

        public PlayersGameControlScheme()
        {
            InputActions.Add(Pause = new InputAction(new InputSource(Button.START), "Pause"));
        }

        private void Start()
        {
            InputDevice = InputDeviceManager.Instance.DefaultInputDevice;
            Active = true;
        }
    }
}