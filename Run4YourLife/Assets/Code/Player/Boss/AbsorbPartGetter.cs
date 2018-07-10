using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    public class AbsorbPartGetter : MonoBehaviour
    {

        [SerializeField]
        private Transform absorbPartTransform;

        public Transform GetPartToAbsorb()
        {
            return absorbPartTransform;
        }
    }
}
