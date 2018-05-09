using Run4YourLife.InputManagement;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public class OptionsMenuControlScheme : ControlScheme
    { 
        public Action Exit { get; private set; }

        public OptionsMenuControlScheme()
        {
            Actions.Add(Exit = new Action(new InputSource(Button.B), "exitAction"));
        }
    }
}