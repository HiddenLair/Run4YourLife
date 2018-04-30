namespace Run4YourLife.Input
{
    public class BossStandControllerControlScheme : PlayerStandControllerControlScheme
    {
        public Action SetAsRunner { get; private set; }

        public BossStandControllerControlScheme()
        {
            Actions.Add(SetAsRunner = new Action(new InputSource(Button.X), "SetAsRunner"));
        }
    }
}