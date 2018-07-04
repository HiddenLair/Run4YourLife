using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Run4YourLife.Player;
using UnityEngine.Events;

namespace Run4YourLife.Interactables
{
    public class BreakableWallController : MonoBehaviour, IRunnerDashBreakable
    {
        [SerializeField]
        private FXReceiver receiver;

        [SerializeField]
        private UnityEvent m_onBrokenByDash;

        public void Break()
        {
            m_onBrokenByDash.Invoke();
            gameObject.SetActive(false);
            receiver.PlayFx();
        }
    }
}
