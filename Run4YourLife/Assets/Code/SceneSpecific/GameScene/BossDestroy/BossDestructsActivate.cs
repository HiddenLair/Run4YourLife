using UnityEngine;
using UnityEngine.Events;

using Run4YourLife.GameManagement;

namespace Run4YourLife.Interactables
{
    public class BossDestructsActivate : BossDestructedInstance {

        [SerializeField]
        private bool m_activateOnRegenerate = true;

        [SerializeField]
        private UnityEvent m_onRegenerated;

        public override void OnBossDestroy()
        {
            gameObject.SetActive(false);
        }

        public override void OnRegenerate()
        {
            gameObject.SetActive(m_activateOnRegenerate);
            m_onRegenerated.Invoke();
        }
    }
}
