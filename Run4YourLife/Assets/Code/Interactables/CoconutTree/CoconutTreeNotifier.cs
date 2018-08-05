using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Run4YourLife;

namespace Run4YourLife.Interactables
{
    public class CoconutTreeNotifier : MonoBehaviour
    {
        private CoconutTreeController m_coconutTreeController;

        private void Awake()
        {
            m_coconutTreeController = transform.parent.GetComponent<CoconutTreeController>();
            Debug.Assert(m_coconutTreeController != null);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Runner))
            {
                m_coconutTreeController.OnRunnerTriggeredCoconutFall();
            }
        }
    }
}