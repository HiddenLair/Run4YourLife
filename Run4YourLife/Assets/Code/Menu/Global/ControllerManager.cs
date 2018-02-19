using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.GameInput
{
    public class ControllerManager : MonoBehaviour
    {
        private List<Controller> controllers = new List<Controller>() {
            new Controller(1),
            new Controller(2),
            new Controller(3),
            new Controller(4)
        };

        public List<Controller> GetControllers()
        {
            return controllers;
        }
    }
}