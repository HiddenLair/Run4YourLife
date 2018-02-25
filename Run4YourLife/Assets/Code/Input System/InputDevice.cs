namespace Run4YourLife.Input
{
    public class InputDevice
    {
        uint id;

        public InputDevice(uint id)
        {
            this.id = id;
        }

        public string InputString(string input)
        {
            return "joy" + id + input;
        }
    }
}