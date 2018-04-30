namespace Run4YourLife.Input
{
    public class Action
    {
        public InputSource InputSource { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }

        public Action(InputSource inputSource, string name)
        {
            this.InputSource = inputSource;
            this.Name = name;
        }

        public bool Started()
        {
            return Enabled && InputSource.ButtonDown();
        }

        public bool Persists()
        {
            return Enabled && InputSource.Button();
        }

        public bool Ended()
        {
            return Enabled && InputSource.ButtonUp();
        }

        public float Value()
        {
            return Enabled ? InputSource.Value() : 0.0f;
        }

        public bool Triggered(float percentage)
        {
            return Enabled && InputSource.Value() >= percentage;
        }
    }
}