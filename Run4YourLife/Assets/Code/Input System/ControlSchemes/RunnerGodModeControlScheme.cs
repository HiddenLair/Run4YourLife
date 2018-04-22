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
            actions.Add(MoveV = new Action(new InputSource(Axis.LEFT_VERTICAL)));
            actions.Add(MoveH = new Action(new InputSource(Axis.LEFT_HORIZONTAL)));
            actions.Add(MSpeed = new Action(new InputSource(Button.A)));
            actions.Add(End = new Action(new InputSource(Button.B)));
        }

        public override void ActionsReactOnPause(PauseState pauseState)
        {
            bool pauseValue = false;
            if(pauseState == PauseState.UNPAUSED)
            {
                pauseValue = true;
            }

            MoveH.enabled = pauseValue;
            MoveV.enabled = pauseValue;
            MSpeed.enabled = pauseValue;
            End.enabled = pauseValue;
        }
    }
}