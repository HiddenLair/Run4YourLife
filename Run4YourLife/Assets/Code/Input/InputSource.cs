namespace Run4YourLife.InputManagement
{
    public enum InputSourceType
    {
        Axis,
        Button,
        Trigger
    }

    public class Axis
    {
        public string ID { get; private set; }

        private Axis(string id) { ID = id; }

        public static readonly Axis LEFT_HORIZONTAL = new Axis("lhor");
        public static readonly Axis LEFT_VERTICAL = new Axis("lver");
        public static readonly Axis RIGHT_HORIZONTAL = new Axis("rhor");
        public static readonly Axis RIGHT_VERTICAL = new Axis("rver");

        public static readonly Axis[] AXES = new Axis[] { LEFT_HORIZONTAL, LEFT_VERTICAL, RIGHT_HORIZONTAL, RIGHT_VERTICAL };
    }

    public class Trigger
    {
        public string ID { get; private set; }

        private Trigger(string id) { ID = id; }

        public static readonly Trigger LEFT = new Trigger("LT");
        public static readonly Trigger RIGHT = new Trigger("RT");

        public static readonly Trigger[] TRIGGERS = new Trigger[] { LEFT, RIGHT };

    }

    public class Button
    {
        public string ID { get; private set; }

        private Button(string id) { ID = id; }

        //public static readonly Button DPAD_LEFT = new Button("dleft");
        //public static readonly Button DPAD_RIGHT = new Button("dright");
        //public static readonly Button DPAD_UP = new Button("dup");
        //public static readonly Button DPAD_DOWN = new Button("ddown");
        public static readonly Button START = new Button("start");
        public static readonly Button SELECT = new Button("select");
        public static readonly Button A = new Button("A");
        public static readonly Button B = new Button("B");
        public static readonly Button X = new Button("X");
        public static readonly Button Y = new Button("Y");
        public static readonly Button RB = new Button("RB");
        public static readonly Button LB = new Button("LB");

        public static readonly Button[] BUTTONS = new Button[] { /*DPAD_LEFT, DPAD_RIGHT, DPAD_UP, DPAD_DOWN,*/ START, SELECT, A, B, X, Y, RB, LB };
    }

    public class InputSource
    {
        private InputSourceType m_inputSourceType;
        private InputDevice m_inputDevice;
        private string m_input;
        private string m_cachedInputString;

        public InputSourceType InputSourceType { get { return m_inputSourceType; } }
        public InputDevice InputDevice { 
            
            get { return m_inputDevice; } 
            set { 
                m_inputDevice = value;
                m_cachedInputString = m_inputDevice.InputString(m_input);
            } 
        }
        public string Input { 
            
            get { return m_input; } 
            set { 
                m_input = value;
                m_cachedInputString = m_inputDevice.InputString(m_input);
            } 
        }

        public InputSource(Axis axis) : this(InputSourceType.Axis, axis.ID) { }
        public InputSource(Button button) : this(InputSourceType.Button, button.ID) { }
        public InputSource(Trigger trigger) : this(InputSourceType.Trigger, trigger.ID) { }

        public InputSource(Axis axis, InputDevice inputDevice) : this(InputSourceType.Axis, axis.ID, inputDevice) { }
        public InputSource(Button button, InputDevice inputDevice) : this(InputSourceType.Button, button.ID, inputDevice) { }
        public InputSource(Trigger trigger, InputDevice inputDevice) : this(InputSourceType.Trigger, trigger.ID, inputDevice) { }

        private InputSource(InputSourceType inputSourceType, string input)
        {
            this.m_inputSourceType = inputSourceType;
            this.m_input = input;
        }

        private InputSource(InputSourceType inputSourceType, string input, InputDevice inputDevice)
        {
            this.m_inputSourceType = inputSourceType;
            this.m_inputDevice = inputDevice;            
            this.m_input = input;
            m_cachedInputString = inputDevice.InputString(input);
        }

        /// <summary>
        /// !!! ALERT !!!
        /// Do NOT use unless you know how to use this!!
        /// </summary>
        /// <param name="inputDevice"></param>
        public InputSource(InputDevice inputDevice)
        {
            this.m_inputDevice = inputDevice;
        }

        public bool ButtonDown()
        {
            return UnityEngine.Input.GetButtonDown(m_cachedInputString);
        }

        public bool Button()
        {
            return UnityEngine.Input.GetButton(m_cachedInputString);
        }

        public bool ButtonUp()
        {
            return UnityEngine.Input.GetButtonUp(m_cachedInputString);
        }

        public float Value()
        {
            return UnityEngine.Input.GetAxis(m_cachedInputString);
        }
    }

}