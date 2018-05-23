using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife;

namespace Run4YourLife.Interactables
{
    public class FallingPlatformNotifier : MonoBehaviour {

        private FallingPlatformController m_fallingPlatformController;

        private void Awake()
        {
            m_fallingPlatformController = transform.parent.GetComponent<FallingPlatformController>();
            Debug.Assert(m_fallingPlatformController);
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(Tags.Runner))
            {
                m_fallingPlatformController.OnRunnerWalkedOnTop();
            }
        }
    }
}
