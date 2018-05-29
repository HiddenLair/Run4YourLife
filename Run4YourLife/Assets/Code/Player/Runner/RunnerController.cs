using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

using Run4YourLife.GameManagement;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(RunnerCharacterController))]
    public class RunnerController : MonoBehaviour, ICharacterEvents
    {

        #region Inspector
        [SerializeField]
        private AudioClip m_deathClip;

        [SerializeField]
        private FXReceiver deathReceiver;

        [SerializeField]
        private float inmuneTimeAfterRevive = 2.0f;

        [SerializeField]
        private float fadePeriodAfterRevive = 0.2f;

        [SerializeField]
        private Renderer[] m_renderersToFade;

        #endregion

        private bool m_isShielded;

        private bool m_reviveMode;

        private bool m_recentlyRevived;

        private GameObject m_shieldGameObject;
        private MaterialFadeOut m_shieldMaterialFadeOut;

        private Coroutine shieldCooldownDestroy;
        private RunnerCharacterController m_runnerCharacterController;      

        private void Awake()
        {
            m_runnerCharacterController = GetComponent<RunnerCharacterController>();
            
            Transform shieldTransform = transform.Find("Graphics/Shield");
            Debug.Assert(shieldTransform != null);
            m_shieldGameObject = shieldTransform.gameObject;
            m_shieldMaterialFadeOut = m_shieldGameObject.GetComponent<MaterialFadeOut>();
        }

        #region Shield

        public void ActivateShield(float shieldTime)
        {
            m_isShielded = true;
            m_shieldGameObject.SetActive(true);
            m_shieldMaterialFadeOut.Activate(shieldTime);
        }

        public void DeactivateShield()
        {
            m_isShielded = false;
            m_shieldGameObject.SetActive(false);
        }

        /// <summary>
        /// Consumes shield if the shield is active
        /// </summary>
        /// <returns>True when it had a shield, false when it did not have a shield</returns>
        public bool ConsumeShieldIfAviable()
        {
            if(m_isShielded)
            {
                Destroy(GetComponent<Shielded>()); // this will call deactivate shield
                return true;
            }

            return false;
        }

        #endregion

        #region Character Effects

        public void Kill()
        {
            AudioManager.Instance.PlaySFX(m_deathClip);

            if (!ConsumeShieldIfAviable() && !m_recentlyRevived)
            {
                deathReceiver.PlayFx();
                gameObject.SetActive(false);
                ExecuteEvents.Execute<IGameplayPlayerEvents>(GameplayPlayerManager.InstanceGameObject, null, (x, y) => x.OnRunnerDeath(gameObject));
            }
        }

        public void AbsoluteKill()
        {
            AudioManager.Instance.PlaySFX(m_deathClip);

            ConsumeShieldIfAviable();
            deathReceiver.PlayFx();
            gameObject.SetActive(false);
            ExecuteEvents.Execute<IGameplayPlayerEvents>(GameplayPlayerManager.InstanceGameObject, null, (x, y) => x.OnRunnerDeath(gameObject));
        }

        public void Impulse(Vector3 force)
        {
            if (!ConsumeShieldIfAviable() && !m_recentlyRevived) {
                m_runnerCharacterController.Impulse(force);
            }
        }

        public void RecentlyRevived()
        {
            m_recentlyRevived = true;
            StartCoroutine(ManageRecentlyRevived());
        }

        IEnumerator ManageRecentlyRevived()
        {
            bool faded = false;
            float timer = Time.time + inmuneTimeAfterRevive;
            float fadeTimer = Time.time + fadePeriodAfterRevive;
            while (timer > Time.time)
            {
                if(fadeTimer <= Time.time)
                {
                    if (faded)
                    {
                        FadeByRender(true);
                    }
                    else
                    {
                        FadeByRender(false);
                    }
                    faded = !faded;
                    fadeTimer = Time.time + fadePeriodAfterRevive;
                }
                yield return null;
            }
            FadeByRender(true);
            m_recentlyRevived = false;
        }

        private void FadeByRender(bool active)
        {
            foreach (Renderer renderer in m_renderersToFade)
            {
                renderer.enabled = active;
            }
        }

        #endregion

        #region Setters
        public void SetReviveMode(bool value)
        {
            m_reviveMode = value;
        }
        #endregion

        #region Getters
        public bool GetReviveMode()
        {
            return m_reviveMode;
        }
        #endregion
    }
}