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
        private bool triggering = false;
        #endregion

        private void OnEnable()
        {
            UICrossHair.Instance.SubscribeWorldCrossHair(gameObject);
        }

        private void OnDisable()
        {
            UICrossHair.Instance.UnsubscribeWorldCrossHair(gameObject);
        }

        private void Update()
        {
            if (!triggering)
            {
                CheckIfSpaceAvailable();
            }
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
            triggering = true;
        }

        private void OnTriggerExit(Collider other)
        {
            Activate();
            triggering = false;
        }

        void CheckIfSpaceAvailable()
        {
            //Check if we have space to place the bigger trap
            RaycastHit info;
            if (Physics.Raycast(transform.position, Vector3.up, out info, maxTrapHeight, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                Deactivate();
            }
            else if (Physics.Raycast(transform.position, Vector3.right, out info, maxTrapWidth / 2, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                Deactivate();
            }
            else if (Physics.Raycast(transform.position, -Vector3.right, out info, maxTrapWidth / 2, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                Deactivate();
            }
            else
            {
                Activate();
            }
        }

        public bool GetActive()
        {
            return active;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 0.2f);
        }
    }
}
