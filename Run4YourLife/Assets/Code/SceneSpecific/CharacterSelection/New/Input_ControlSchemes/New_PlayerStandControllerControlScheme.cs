namespace Run4YourLife.InputManagement
{
    public class New_PlayerStandControllerControlScheme : ControlScheme
    {
        public InputAction Select { get; private set; }
        public InputAction Unselect { get; private set; }
        public InputAction Rotate { get; private set; }
        public InputAction VerticalStand { get; private set; }
        public InputAction HorizontalStand { get; private set; }

        public New_PlayerStandControllerControlScheme()
        {
            InputActions.Add(Select = new InputAction(new InputSource(Button.A), "Select"));
            InputActions.Add(Unselect = new InputAction(new InputSource(Button.B), "Unselect"));
            InputActions.Add(Rotate = new InputAction(new InputSource(Axis.LEFT_HORIZONTAL), "Rotate"));
            InputActions.Add(VerticalStand = new InputAction(new InputSource(Axis.LEFT_VERTICAL), "VerticalStand"));
            InputActions.Add(HorizontalStand = new InputAction(new InputSource(Axis.LEFT_HORIZONTAL), "HorizontalStand"));
        }
    }
}