using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Run4YourLife.Input
{
    public class InputDeviceManager : MonoBehaviour
    {
        List<InputDevice> inputDevices = new List<InputDevice>()
        {
            new InputDevice(1),
            new InputDevice(2),
            new InputDevice(3),
            new InputDevice(4)
        };

        public List<InputDevice> InputDevices()
        {
            return inputDevices;
        }
    }
}

