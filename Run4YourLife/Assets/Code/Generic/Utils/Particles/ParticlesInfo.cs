using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Utils
{
    public class ParticlesInfo : MonoBehaviour
    {
        //This class is just a support class where we can store info about particles, and other scripts will use
        [Tooltip("Variable to indicate that particle scaler should scale the shape of this particle system, instead of its transform")]
        public bool setScaleAsShape = false;
        [Tooltip("Variable to indicate if position should be modified at same time than scale, in order to scale full just on one side, not half on both sides")]
        public bool adjustPositionOnScaling = false;
    }
}
