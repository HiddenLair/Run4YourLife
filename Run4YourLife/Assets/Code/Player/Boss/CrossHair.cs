using UnityEngine;

namespace Run4YourLife.Player
{
    public class CrossHair : MonoBehaviour
    {
        private bool m_isOperative = true;
        private Collider collidingObject = null;

        public bool IsOperative { get { return m_isOperative; } set { m_isOperative = value; } }
        
        private void OnTriggerEnter(Collider other)
        {
            m_isOperative = false;
            collidingObject = other;
        }

        private void OnTriggerExit(Collider other)
        {
            /*

            If 2 objects are in contact (extreme case: the 2nd one is partially inside of the 1st one) it can happen that
            the OnTriggerEnter of the 2nd one triggers before the OnTriggerExit of the 1st one
            Update m_isOperative only if we are exiting the previously registered collider
            If that is not the case, then this will be managed inside Update

            */

            if(collidingObject == other)
            {
                m_isOperative = true;
                collidingObject = null;
            }
        }

        private void Update()
        {
            if(!m_isOperative && (!collidingObject || !collidingObject.gameObject.activeInHierarchy))
            {
                m_isOperative = true;
                collidingObject = null;
            }
        }
    }
}