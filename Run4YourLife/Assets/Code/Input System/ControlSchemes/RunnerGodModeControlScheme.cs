using Run4YourLife.GameManagement;

namespace Run4YourLife.Input
{
    public class RunnerGodModeControlScheme : PlayerControlScheme
    {
        public Action MoveH { get; private set; }
        public Action MoveV { get; private set; }
        public Action MSpeed { get; private set; }
        public Action End { get; private set; }

        public RunnerGodModeControlScheme()
        {
            Actions.Add(MoveV = new Action(new InputSource(Axis.LEFT_VERTICAL), "MoveV"));
            Actions.Add(MoveH = new Action(new InputSource(Axis.LEFT_HORIZONTAL), "MoveH"));
            Actions.Add(MSpeed = new Action(new InputSource(Button.A), "MSpeed"));
            Actions.Add(End = new Action(new InputSource(Button.B), "End"));
        }

        public override void ActionsReactOnPause(PauseState pauseState)
        {
            bool pauseValue = false;
            if(pauseState == PauseState.UNPAUSED)
            {
                pauseValue = true;
            }

            MoveH.Enabled = pauseValue;
            MoveV.Enabled = pauseValue;
            MSpeed.Enabled = pauseValue;
            End.Enabled = pauseValue;
        }
    }
}