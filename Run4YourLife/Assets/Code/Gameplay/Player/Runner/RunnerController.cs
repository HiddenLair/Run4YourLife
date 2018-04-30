using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.GameManagement;


namespace Run4YourLife.Player
{
    [RequireComponent(typeof(RunnerCharacterController))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BuffManager))]
    public class RunnerController : MonoBehaviour, ICharacterEvents
    {
        private RunnerCharacterController m_runnerCharacterController;
        private BuffManager m_buffManager;

        private void Awake()
        {
            m_runnerCharacterController = GetComponent<RunnerCharacterController>();
            m_buffManager = GetComponent<BuffManager>();
        }

        private bool HasShield()
        {
            RunnerState buff = m_buffManager.GetBuff();
            return buff != null && buff.StateType == RunnerState.Type.Shielded;
        }

        private bool ConsumeShieldIfAviable()
        {
            if(HasShield())
            {
                Destroy(m_buffManager.GetBuff());
                return true;
            }
            return false;
        }

        #region Character Effects

        public void Kill()
        {
            if (!ConsumeShieldIfAviable())
            {
                Instantiate(m_runnerCharacterController.deathParticles, transform.position, transform.rotation);
                ExecuteEvents.Execute<IGameplayPlayerEvents>(GameplayPlayerManager.InstanceGameObject, null, (x, y) => x.OnRunnerDeath(gameObject));
            }
        }

        public void Impulse(Vector3 force)
        {
            if (!ConsumeShieldIfAviable()) {
                m_runnerCharacterController.Impulse(force);
            }
        }

        public void Root(int rootHardness)
        {
            if (!ConsumeShieldIfAviable())
            {
                Root oldInstance = gameObject.GetComponent<Root>();
                if (oldInstance != null)
                {
                    Destroy(oldInstance);
                }
                gameObject.AddComponent<Root>().SetHardness(rootHardness);
            }
        }

        public void AbsoluteKill()
        {
            ExecuteEvents.Execute<IGameplayPlayerEvents>(GameplayPlayerManager.InstanceGameObject, null, (x, y) => x.OnRunnerDeath(gameObject));
        }

        #endregion
    }
}