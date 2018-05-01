using Run4YourLife.Input;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public class ExitButtonControlScheme : ControlScheme
    { 
        public Action ExitAction { get; private set; }

        public ExitButtonControlScheme()
        {
            Actions.Add(ExitAction = new Action(new InputSource(Button.B), "exitAction"));
        }
    }
}