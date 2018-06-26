using UnityEngine;
using UnityEngine.Events;

namespace Run4YourLife.Utils
{
    public class TriggerEventNotifier : MonoBehaviour
    {
        [SerializeField]
        private LayerMask layersToTest;
        [SerializeField]
        private string tagToTest;
        public UnityEvent triggerEnter;
        public UnityEvent triggerExit;

        private void OnTriggerEnter(Collider other)
        {
            Check(other.gameObject,triggerEnter);
        }

        private void OnTriggerExit(Collider other)
        {
            Check(other.gameObject,triggerExit);
        }

        private void Check(GameObject g, UnityEvent e)
        {
            if (((1 << g.layer) & layersToTest) != 0)
            {
                if(tagToTest != "")
                {
                    if (g.CompareTag(tagToTest))
                    {
                        e.Invoke();
                    }
                }
                else
                {
                    e.Invoke();
                }
            }
        }
    }
}