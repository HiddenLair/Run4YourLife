﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.Input;

namespace Run4YourLife.Player {
    public class ResetPlayerStatus : MonoBehaviour {

        public void Reset()
        {
            GetComponent<RunnerInputStated>().Clear();
            GetComponent<BuffManager>().Clear();
            GetComponent<Stats>().Clear();
            GetComponent<RunnerCharacterController>().Clear();
        }
    }
}
