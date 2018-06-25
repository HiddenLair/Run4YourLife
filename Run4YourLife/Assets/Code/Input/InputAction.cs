namespace Run4YourLife.InputManagement
{
    public class InputAction
    {
        public InputSource InputSource { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }

        /// <summary>
        /// Last value where value != 0
        /// </summary>
        public float LastValue { get; private set; } 

        public InputAction(InputSource inputSource, string name)
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
            float value = Enabled ? InputSource.Value() : 0.0f;
            LastValue = value != 0 ? value : LastValue;
            return value;
        }

        public bool BoolValue(float threshold = 0.2f)
        {
            return Value() > threshold;
        }

        public bool Triggered(float percentage)
        {
            return Enabled && InputSource.Value() >= percentage;
        }
    }
}