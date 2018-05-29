using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.Interactables
{
    public class RootTrapControl : TrapBase
    {
        [SerializeField]
        private GameObject m_activationParticles;

        [SerializeField]
        private int m_interactionsUntilRelease;

        [SerializeField]
        private StatusEffectSet m_statusEffectSet;

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(Tags.Runner))
            {
                GameObject runner = collider.gameObject;

                Root root = runner.gameObject.GetComponent<Root>();
                if (root == null)
                {
                    root = runner.AddComponent<Root>();
                    root.Init(m_interactionsUntilRelease, m_statusEffectSet);
                }

                root.RefreshRoot();
                AudioManager.Instance.PlaySFX(m_trapDetonationClip);
                gameObject.SetActive(false);
            }
        }
    }
}