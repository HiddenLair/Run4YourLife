using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife;

namespace Run4YourLife.Interactables
{
    public class FallingPlatformRunnerNotifier : MonoBehaviour
    {
        [SerializeField]
        private FallingPlatformController m_fallingPlatformController;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(Tags.Runner))
            {
                m_fallingPlatformController.OnRunnerWalkedOnTop();
            }
        }
    }
}
