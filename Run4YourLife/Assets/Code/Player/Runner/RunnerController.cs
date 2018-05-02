using UnityEngine;
using UnityEngine.EventSystems;

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

        #endregion
        private bool m_isShielded;

        private Transform m_shieldGameObject;
        private RunnerCharacterController m_runnerCharacterController;

        private void Awake()
        {
            m_runnerCharacterController = GetComponent<RunnerCharacterController>();

            m_shieldGameObject = transform.Find("Graphics/Shield");
            UnityEngine.Debug.Assert(m_shieldGameObject != null, "Shield gameobject has not been found inside the character");
        }

        #region Shield

        public void ActivateShield()
        {
            m_isShielded = true;
            m_shieldGameObject.gameObject.SetActive(true);
        }

        /// <summary>
        /// Consumes shield if the shield is active
        /// </summary>
        /// <returns>True when it had a shield, false when it did not have a shield</returns>
        public bool ConsumeShieldIfAviable()
        {
            bool wasShielded = m_isShielded;

            m_isShielded = false;
            m_shieldGameObject.gameObject.SetActive(false);

            return wasShielded;
        }

        #endregion

        #region Character Effects

        public void Kill()
        {
            if (!ConsumeShieldIfAviable())
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
            if (!ConsumeShieldIfAviable()) {
                m_runnerCharacterController.Impulse(force);
            }
        }

        #endregion
    }
}