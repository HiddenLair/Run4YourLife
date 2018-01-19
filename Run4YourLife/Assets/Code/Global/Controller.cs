using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.GameInput
{
    public class Controller
    {
        public class Axis
        {
            public string ID { get; private set; }

            public Axis(string id) { ID = id; }

            public static readonly Axis LEFT_HORIZONTAL = new Axis("lhor");
            public static readonly Axis LEFT_VERTICAL = new Axis("lver");
            public static readonly Axis RIGHT_HORIZONTAL = new Axis("rhor");
            public static readonly Axis RIGHT_VERTICAL = new Axis("rver");
        }

        public class Trigger
        {
            public string ID { get; private set; }

            public Trigger(string id) { ID = id; }

            public static readonly Trigger LEFT = new Trigger("ltrig");
            public static readonly Trigger RIGHT = new Trigger("rtrig");
        }

        public class Button
        {
            public string ID { get; private set; }

            public Button(string id) { ID = id; }

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
            public static readonly Button R = new Button("R");
            public static readonly Button L = new Button("L");
        }

        private const string JOYSTICK = "joy";

        public int controllerId;

        public Controller(int id)
        {
            controllerId = id;
        }


        private string GetButtonString(Button button)
        {
            return JOYSTICK + controllerId + button.ID;
        }

        public bool GetButton(Button button)
        {
            return Input.GetButton(GetButtonString(button));
        }

        public bool GetButtonDown(Button button)
        {
            return Input.GetButtonDown(GetButtonString(button));
        }

        public bool GetButtonUp(Button button)
        {
            return Input.GetButtonUp(GetButtonString(button));
        }

        private string GetTriggerString(Trigger trigger)
        {
            return JOYSTICK + controllerId + trigger.ID;
        }

        public float GetTrigger(Trigger trigger)
        {
            return Input.GetAxis(GetTriggerString(trigger));
        }

        private string GetAxisString(Axis axis)
        {
            return JOYSTICK + controllerId + axis.ID;
        }

        public float GetAxis(Axis axis)
        {
            return Input.GetAxis(GetAxisString(axis));
        }
    }
}
