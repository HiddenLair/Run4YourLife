namespace Run4YourLife.Input
{
    public class BossStandControllerControlScheme : PlayerStandControllerControlScheme
    {
        public Action setAsRunner;

        protected override void OnAwake()
        {
            actions.Add(setAsRunner = new Action(new InputSource(Button.X)));
        }
    }
}