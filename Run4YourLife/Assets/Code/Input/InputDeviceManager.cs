using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Run4YourLife.Input
{
    public class InputDeviceManager : SingletonMonoBehaviour<InputDeviceManager>
    {
        private InputDevice m_defaultInputDevice = new InputDevice(0);
        private List<InputDevice> m_inputDevices = new List<InputDevice>(new InputDevice[] {
                        new InputDevice(1),
                        new InputDevice(2),
                        new InputDevice(3),
                        new InputDevice(4),
                        new InputDevice(5),
                    });

        /// <summary>
        /// InputDevice that captures input from all sources
        /// </summary>
        public InputDevice DefaultInputDevice { get { return m_defaultInputDevice; } }

        /// <summary>
        /// List Of All Input Devices
        /// [0]=Keyboard, [1,4]=Joysticks 
        /// </summary>
        public List<InputDevice> InputDevices { get { return m_inputDevices; } }
    }
}
