using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Run4YourLife.Input
{
    public class InputDeviceManager : MonoBehaviour
    {
        List<InputDevice> m_inputDevices = new List<InputDevice>();
        List<InputDevice> m_inactiveInputDevices = new List<InputDevice>();
        List<InputDevice> m_activeInputDevices = new List<InputDevice>();

        public List<InputDevice> ActiveInputDevices { get { return m_activeInputDevices; } }
        public List<InputDevice> InactiveInputDevices { get { return m_activeInputDevices; } }

        public InputDeviceManager()
        {
            m_inputDevices.AddRange(
                    new InputDevice[] {
                        new InputDevice(1),
                        new InputDevice(2),
                        new InputDevice(3),
                        new InputDevice(4),
                    }
                );

            ResetInputDevices();
        }

        private void Awake()
        {
            enabled = false;
        }

        public List<InputDevice> InputDevices()
        {
            return m_inputDevices;
        }

        public void ResetInputDevices()
        {
            m_inactiveInputDevices.AddRange(m_inputDevices);
            m_activeInputDevices.Clear();
        }

        public void StartInputDeviceDetection()
        {
            enabled = true;
        }

        public void StopInputDeviceDetection()
        {
            enabled = false;
        }

        private void Update()
        {
            foreach(InputDevice inputDevice in m_inactiveInputDevices)
            {
                if(DetectInputDeviceInput(inputDevice))
                {
                    m_activeInputDevices.Add(inputDevice);
                }
            }

            m_inactiveInputDevices.RemoveAll((x) => m_activeInputDevices.Contains(x));
        }

        private bool DetectInputDeviceInput(InputDevice inputDevice)
        {
            InputSource inputSource = new InputSource(inputDevice);

            foreach (Trigger trigger in Trigger.TRIGGERS)
            {
                inputSource.input = trigger.ID;
                if(inputSource.Value() != 0)
                {
                    return true;
                }
            }

            foreach (Axis axis in Axis.AXES)
            {
                inputSource.input = axis.ID;
                if (inputSource.Value() != 0)
                {
                    return true;
                }
            }

            foreach (Button button in Button.BUTTONS)
            {
                inputSource.input = button.ID;
                if (inputSource.ButtonDown())
                {
                    return true;
                }
            }

            return false;
        }
    }
}

