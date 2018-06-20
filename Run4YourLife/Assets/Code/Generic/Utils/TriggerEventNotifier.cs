using UnityEngine;
using UnityEngine.Events;

namespace Run4YourLife.Utils
{
    public class TriggerEventNotifier : MonoBehaviour
    {
        public UnityEvent triggerEnter;
        public UnityEvent triggerExit;

        private void OnTriggerEnter(Collider other)
        {
            triggerEnter.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            triggerExit.Invoke();
        }
    }
}