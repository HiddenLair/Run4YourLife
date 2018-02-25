using UnityEngine;

namespace Run4YourLife.Input
{
    public class Axis
    {
        public string ID { get; private set; }

        private Axis(string id) { ID = id; }

        public static readonly Axis LEFT_HORIZONTAL = new Axis("lhor");
        public static readonly Axis LEFT_VERTICAL = new Axis("lver");
        public static readonly Axis RIGHT_HORIZONTAL = new Axis("rhor");
        public static readonly Axis RIGHT_VERTICAL = new Axis("rver");
    }

    public class Trigger
    {
        public string ID { get; private set; }

        private Trigger(string id) { ID = id; }

        public static readonly Trigger LEFT = new Trigger("LT");
        public static readonly Trigger RIGHT = new Trigger("RT");
    }

    public class Button
    {
        public string ID { get; private set; }

        private Button(string id) { ID = id; }

        public static readonly Button DPAD_LEFT = new Button("dleft");
        public static readonly Button DPAD_RIGHT = new Button("dright");
        public static readonly Button DPAD_UP = new Button("dup");
        public static readonly Button DPAD_DOWN = new Button("ddown");
        public static readonly Button START = new Button("start");
        public static readonly Button SELECT = new Button("select");
        public static readonly Button A = new Button("A");
        public static readonly Button B = new Button("B");
        public static readonly Button X = new Button("X");
        public static readonly Button Y = new Button("Y");
        public static readonly Button RB = new Button("RB");
        public static readonly Button LB = new Button("LB");
    }

    public class InputSource
    {
        private string input;
        private InputDevice inputDevice;

        public InputSource(Axis axis, InputDevice inputDevice) : this(axis.ID, inputDevice) { }
        public InputSource(Button button, InputDevice inputDevice) : this(button.ID, inputDevice) { }
        public InputSource(Trigger trigger, InputDevice inputDevice) : this(trigger.ID, inputDevice) { }

        private InputSource(string input, InputDevice inputDevice)
        {
            this.input = input;
            this.inputDevice = inputDevice;
        }

        public bool ButtonDown()
        {
            return UnityEngine.Input.GetButtonDown(inputDevice.InputString(input));
        }

        public bool Button()
        {
            return UnityEngine.Input.GetButton(inputDevice.InputString(input));
        }

        public bool ButtonUp()
        {
            return UnityEngine.Input.GetButtonUp(inputDevice.InputString(input));
        }

        public float Value()
        {
            return UnityEngine.Input.GetAxis(inputDevice.InputString(input));
        }
    }

}