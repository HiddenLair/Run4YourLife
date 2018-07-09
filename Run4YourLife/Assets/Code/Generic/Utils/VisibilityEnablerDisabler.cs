using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Utils
{
    public class VisibilityEnablerDisabler : MonoBehaviour {
        
        [SerializeField]
        private MonoBehaviour m_monoBehaviour;

        private void OnBecameInvisible()
        {
            m_monoBehaviour.enabled = false;
        }

        private void OnBecameVisible()
        {
            m_monoBehaviour.enabled = true;
        }
    }
}
