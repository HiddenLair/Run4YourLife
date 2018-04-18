using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Run4YourLife.Input
{
    public class InputDeviceManager : MonoBehaviour
    {
        InputDevice m_defaultInputDevice = new InputDevice(0);
        List<InputDevice> m_inputDevices = new List<InputDevice>(new InputDevice[] {
                        new InputDevice(1),
                        new InputDevice(2),
                        new InputDevice(3),
                        new InputDevice(4),
                        new InputDevice(5),
                    });

        List<InputDevice> m_inactiveInputDevices = new List<InputDevice>();
        List<InputDevice> m_activeInputDevices = new List<InputDevice>();

        public InputDevice DefaultInputDevice { get { return m_defaultInputDevice; } }
        public List<InputDevice> InputDevices { get { return m_inputDevices; } }
        public List<InputDevice> ActiveInputDevices { get { return m_activeInputDevices; } }
        public List<InputDevice> InactiveInputDevices { get { return m_activeInputDevices; } }

        public InputDeviceManager()
        {
            ResetInputDevices();
        }

        private void Awake()
        {
            enabled = false;
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
                if(inputDevice.HasInput())
                {
                    m_activeInputDevices.Add(inputDevice);
                }
            }

            m_inactiveInputDevices.RemoveAll((x) => m_activeInputDevices.Contains(x));
        }
    }
}
