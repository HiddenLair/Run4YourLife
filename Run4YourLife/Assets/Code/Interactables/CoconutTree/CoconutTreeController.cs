using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.Utils;

namespace Run4YourLife.Interactables
{
    public class CoconutTreeController : MonoBehaviour
    {

        [SerializeField]
        private float m_timeToReappear;

        private CoconutController m_coconutController;

        private void Awake()
        {
            m_coconutController = GetComponentInChildren<CoconutController>();
            Debug.Assert(m_coconutController != null);
        }

        public void OnRunnerTriggeredCoconutFall()
        {
            if (m_coconutController.IsIdle)
            {
                m_coconutController.Fall();
                StartCoroutine(YieldHelper.WaitForSeconds(() => ResetCoconut(), m_timeToReappear));
            }
        }

        public void ResetCoconut()
        {
            m_coconutController.Reset();
        }
    }
}