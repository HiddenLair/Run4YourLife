using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.UI {
    public class CreditsSwitcher : MonoBehaviour {

        [SerializeField]
        private GameObject optionsCanvas;

        [SerializeField]
        private GameObject creditsCanvas;

        private bool switchState = true;

        public void Switch()
        {
            switchState = !switchState;
            optionsCanvas.SetActive(switchState);
            creditsCanvas.SetActive(!switchState);
        }

    }
}