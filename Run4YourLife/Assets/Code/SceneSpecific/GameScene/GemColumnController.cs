using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.SceneSpecific
{
    public class GemColumnController : MonoBehaviour
    {

        [SerializeField]
        private GameObject gem;

        [SerializeField]
        private GameObject fire;

        public void ActivateColumn()
        {
            gem.SetActive(true);
            fire.SetActive(true);
        }

        public void DeactivateColumn()
        {
            gem.SetActive(false);
            fire.SetActive(false);
        }
    }
}
