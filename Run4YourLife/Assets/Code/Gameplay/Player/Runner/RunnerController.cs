using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Run4YourLife.Input;
using Run4YourLife.Player;
using Run4YourLife.GameManagement;


namespace Run4YourLife.Player
{
    [RequireComponent(typeof(RunnerCharacterController))]
    [RequireComponent(typeof(Stats))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BuffManager))]
    public class RunnerController : MonoBehaviour, ICharacterEvents
    {
        #region References

        private Stats m_stats;
        private RunnerCharacterController m_runnerCharacterController;
        private BuffManager m_buffManager;
        #endregion

        private void Awake()
        {
            m_runnerCharacterController = GetComponent<RunnerCharacterController>();
            m_stats = GetComponent<Stats>();
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

        public void Debuff(StatModifier statmodifier)
        {
            if (!ConsumeShieldIfAviable()) {
                m_stats.AddModifier(statmodifier);
            }
        }

        public void Burned(int burnedTime)
        {
            Burned oldInstance = gameObject.GetComponent<Burned>();
            if (oldInstance != null)
            {
                oldInstance.Refresh();
            }
            else
            {
                gameObject.AddComponent<Burned>().SetBurningTime(burnedTime);
            }
        }

        public void ActivateWind(float windForce,ref Wind component)
        {
            component = gameObject.AddComponent<Wind>();
            component.AddWindForce(windForce);
        }

        public void DeactivateWind(float windForce,Wind component)
        {
            Destroy(component);
        }

        public void AbsoluteKill()
        {
            ExecuteEvents.Execute<IGameplayPlayerEvents>(GameplayPlayerManager.InstanceGameObject, null, (x, y) => x.OnRunnerDeath(gameObject));
        }

        #endregion
    }
}