using UnityEngine;

using Run4YourLife.UI;

namespace Run4YourLife.Player
{
    public class CrossHair : MonoBehaviour
    {        
        
        private bool m_isOperative = true;


        public bool IsOperative { get { return m_isOperative; } set { m_isOperative = value; } }
        
        private void OnTriggerEnter(Collider other)
        {
            m_isOperative = false;
        }

        private void OnTriggerExit(Collider other)
        {
            m_isOperative = true;
        }
    }
}
