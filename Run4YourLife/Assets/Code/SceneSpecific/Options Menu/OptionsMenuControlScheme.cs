using Run4YourLife.InputManagement;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public class OptionsMenuControlScheme : ControlScheme
    { 
        public InputAction Exit { get; private set; }

        public OptionsMenuControlScheme()
        {
            InputActions.Add(Exit = new InputAction(new InputSource(Button.B), "exitAction"));
        }
    }
}