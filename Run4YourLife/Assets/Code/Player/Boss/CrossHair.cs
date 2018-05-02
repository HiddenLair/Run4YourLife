using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    public class CrossHair : MonoBehaviour
    {

        #region Inspector

        [SerializeField]
        private float maxTrapWidth;

        [SerializeField]
        private float maxTrapHeight;

        #endregion

        #region Variables
        private bool active = true;
        #endregion


        //TODO: this creates exceptions when closing the editor
        private void OnEnable()
        {
            UICrossHair.Instance.SubscribeWorldCrossHair(gameObject);
        }

        private void OnDisable()
        {
            UICrossHair.Instance.UnsubscribeWorldCrossHair(gameObject);
        }

        void Deactivate()
        {
            //There is no space to place the trap
            active = false;
        }

        void Activate()
        {
            active = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            Deactivate();
        }

        private void OnTriggerExit(Collider other)
        {
            Activate();
        }

        public bool GetActive()
        {
            return active;
        }
    }
}
