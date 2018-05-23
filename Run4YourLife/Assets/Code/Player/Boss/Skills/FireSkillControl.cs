using System.Collections;
using UnityEngine;

namespace Run4YourLife.Player
{
    public class FireSkillControl : SkillBase
    {
        [SerializeField]
        [Tooltip("Time it take for the skill to disappear")]
        private float m_skillTime;

        [SerializeField]
        [Tooltip("How long the player is burnt")]
        private float m_burnTime;

        [SerializeField]
        private StatusEffectSet m_burnStatusEffectSet;

        private void OnEnable()
        {
            StartCoroutine(DeactivateAfterTime());
        }

        IEnumerator DeactivateAfterTime()
        {
            yield return new WaitForSeconds(m_skillTime);
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(Tags.Runner))
            {
                GameObject runner = collider.gameObject;

                Burned burned = runner.gameObject.GetComponent<Burned>();
                if (burned == null)
                {
                    burned = runner.AddComponent<Burned>();
                    burned.Init(m_burnTime, m_burnStatusEffectSet);
                }

                burned.RefreshTime();
            }
        }
    }
}