using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.Player;
using UnityEngine.Events;

namespace Run4YourLife.Interactables
{
    public class BreakByDash : MonoBehaviour
    {
        [SerializeField]
        private FXReceiver receiver;

        [SerializeField]
        private UnityEvent m_onBrokenByDash;

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Tags.Runner))
            {
                if (other.GetComponent<RunnerCharacterController>().IsDashing)
                {
                    Break();
                }
            }
        }

        public void Break()
        {
            m_onBrokenByDash.Invoke();
            gameObject.SetActive(false);
            receiver.PlayFx();
        }
    }
}
