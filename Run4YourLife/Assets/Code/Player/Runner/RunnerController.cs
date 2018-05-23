using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

using Run4YourLife.GameManagement;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(RunnerCharacterController))]
    public class RunnerController : MonoBehaviour, ICharacterEvents
    {

        #region Inspector
        [SerializeField]
        private FXReceiver bodyReceiver;

        [SerializeField]
        private GameObject deathPartciles;

        [SerializeField]
        private float inmuneTimeAfterRevive = 2.0f;

        [SerializeField]
        private float fadePeriodAfterRevive = 0.2f;

        #endregion

        private bool m_isShielded;

        private bool m_reviveMode;

        private bool m_recentlyRevived;

        private Renderer[] m_renderers;
        private Transform m_shieldGameObject;
        private Coroutine shieldCooldownDestroy;
        private RunnerCharacterController m_runnerCharacterController;      

        private void Awake()
        {
            m_runnerCharacterController = GetComponent<RunnerCharacterController>();

            m_renderers = gameObject.GetComponentsInChildren<Renderer>();
            m_shieldGameObject = transform.Find("Graphics/Shield");
            Debug.Assert(m_shieldGameObject != null, "Shield gameobject has not been found inside the character");
        }

        #region Shield

        public void ActivateShield(float shieldTime)
        {
            m_isShielded = true;
            m_shieldGameObject.gameObject.SetActive(true);
            m_shieldGameObject.GetComponent<MaterialFadeOut>().Activate(shieldTime);

            if(shieldCooldownDestroy != null)
            {
                StopCoroutine(shieldCooldownDestroy);
            }

            shieldCooldownDestroy = StartCoroutine(DestroyShieldOnCooldownReached(shieldTime));
        }

        /// <summary>
        /// Consumes shield if the shield is active
        /// </summary>
        /// <returns>True when it had a shield, false when it did not have a shield</returns>
        public bool ConsumeShieldIfAviable()
        {
            if (shieldCooldownDestroy != null)
            {
                StopCoroutine(shieldCooldownDestroy);
            }

            bool wasShielded = m_isShielded;

            m_isShielded = false;
            m_shieldGameObject.gameObject.SetActive(false);

            return wasShielded;
        }

        /// <summary>
        /// Consumes shield if enough time passes
        /// </summary>
        IEnumerator DestroyShieldOnCooldownReached(float shieldTime)
        {
            yield return new WaitForSeconds(shieldTime);
            ConsumeShieldIfAviable();
        }


        #endregion

        #region Character Effects

        public void Kill()
        {
            if (!ConsumeShieldIfAviable() && !m_recentlyRevived)
            {
                bodyReceiver.PlayFx(deathPartciles);
                gameObject.SetActive(false);
                ExecuteEvents.Execute<IGameplayPlayerEvents>(GameplayPlayerManager.InstanceGameObject, null, (x, y) => x.OnRunnerDeath(gameObject));
            }
        }

        public void AbsoluteKill()
        {
            bodyReceiver.PlayFx(deathPartciles);
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
            foreach (Renderer renderer in m_renderers)
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