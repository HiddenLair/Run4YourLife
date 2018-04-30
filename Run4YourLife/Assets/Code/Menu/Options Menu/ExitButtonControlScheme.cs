namespace Run4YourLife.Input
{
    public class ExitButtonControlScheme : ControlScheme
    { 
        public Action exitAction { get; private set; }

        public ExitButtonControlScheme()
        {
            Actions.Add(exitAction = new Action(new InputSource(Button.B), "exitAction"));
        }
    }
}