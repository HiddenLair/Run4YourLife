using UnityEngine;
using UnityEngine.Events;

namespace Run4YourLife.Utils
{
    public class ExecuteEventAfterSeconds : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent m_toExecute;

        [SerializeField]
        private float m_seconds;

        public void Invoke()
        {
            StartCoroutine(YieldHelper.WaitForSeconds(m_toExecute.Invoke, m_seconds));
        }
    }
}