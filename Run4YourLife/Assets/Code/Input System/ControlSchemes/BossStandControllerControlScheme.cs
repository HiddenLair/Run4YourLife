namespace Run4YourLife.Input
{
    public class BossStandControllerControlScheme : PlayerStandControllerControlScheme
    {
        public Action setAsRunner;

        public BossStandControllerControlScheme()
        {
            actions.Add(setAsRunner = new Action(new InputSource(Button.X)));
        }
    }
}