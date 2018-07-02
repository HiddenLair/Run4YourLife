using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player.Runner.Ghost
{
    public class RunnerGhostTriggerNotifier : MonoBehaviour {

        private RunnerGhostController m_runnerGhostController;
        
        private void Awake()
        {
            m_runnerGhostController = GetComponentInParent<RunnerGhostController>();
            Debug.Assert(m_runnerGhostController != null);
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(Tags.Runner))
            {
                m_runnerGhostController.OnOtherRunnerCollidedGhost();
            }
        }
    }
}
