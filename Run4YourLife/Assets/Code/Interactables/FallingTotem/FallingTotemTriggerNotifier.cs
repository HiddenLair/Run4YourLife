using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Interactables
{
    public class FallingTotemTriggerNotifier : MonoBehaviour {

        [SerializeField]
        private FallingTotemController m_totemController;

        private void Reset()
        {
            m_totemController = transform.parent.GetComponent<FallingTotemController>();
            Debug.Assert(m_totemController != null);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Runner))
            {
                m_totemController.OnRunnerTriggeredTotemFall();
            }
        }
    }
}

