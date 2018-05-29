namespace Run4YourLife.InputManagement
{
    public class BossStandControllerControlScheme : PlayerStandControllerControlScheme
    {
        public InputAction SetAsRunner { get; private set; }

        public BossStandControllerControlScheme()
        {
            InputActions.Add(SetAsRunner = new InputAction(new InputSource(Button.X), "SetAsRunner"));
        }
    }
}