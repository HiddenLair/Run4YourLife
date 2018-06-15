﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.Player;
using UnityEngine.Events;

namespace Run4YourLife.Interactables
{
    public class BreakByDash : MonoBehaviour
    {
        [SerializeField]
        private FXReceiver receiver;

        [SerializeField]
        private UnityEvent m_onBrokenByDash;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(Tags.Runner))
            {
                if (other.GetComponent<RunnerCharacterController>().IsDashing)
                {
                    Break();
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Tags.Runner))
            {
                if (other.GetComponent<RunnerCharacterController>().IsDashing)
                {
                    Break();
                }
            }
        }

        public void ManualBreak()
        {
            Break();
        }

        private void Break()
        {
            if (receiver != null)
            {
                receiver.PlayFx();
            }

            if (m_onBrokenByDash.GetPersistentEventCount() == 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                m_onBrokenByDash.Invoke();
            }
        }
    }
}
