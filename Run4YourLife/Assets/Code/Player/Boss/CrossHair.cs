using UnityEngine;

using Run4YourLife.UI;

namespace Run4YourLife.Player
{
    public class CrossHair : MonoBehaviour
    {
        private Collider collidingObject = null;

        private bool m_isOperative = true;

        public bool IsOperative { get { return m_isOperative; } set { m_isOperative = value; } }
        
        private void OnTriggerEnter(Collider other)
        {
            m_isOperative = false;
            collidingObject = other;
        }

        private void Update()
        {
            if(!m_isOperative && !collidingObject)
            {
                m_isOperative = true;
            }
        }
    }
}
