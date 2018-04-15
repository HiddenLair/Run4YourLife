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
    [RequireComponent(typeof(Wind))]
    public class RunnerController : MonoBehaviour, ICharacterEvents
    {
        #region References

        private GameplayPlayerManager m_gameplayPlayerManager;
        private Stats m_stats;
        private RunnerCharacterController m_runnerCharacterController;
        private RunnerInputStated inputPlayer;
        private Wind m_wind;

        #endregion

        private void Awake()
        {
            m_runnerCharacterController = GetComponent<RunnerCharacterController>();
            m_stats = GetComponent<Stats>();
            inputPlayer = GetComponent<RunnerInputStated>();
            m_wind = GetComponent<Wind>();
            m_gameplayPlayerManager = FindObjectOfType<GameplayPlayerManager>();
            Debug.Assert(m_gameplayPlayerManager != null);
        }

        private void OnTriggerStay(Collider collider)
        {
            if (collider.CompareTag(Tags.Interactable) && inputPlayer.GetDashInput())
            {
                Interact(collider.gameObject);
            }
        }
        private void Interact(GameObject gameObject)
        {
            ExecuteEvents.Execute<IInteractableEvents>(gameObject, null, (x, y) => x.Interact());
        }

        private bool CheckForShield()
        {
            bool ret = false;

            RunnerState buff = GetComponent<BuffManager>().GetBuff();
            if (buff != null && buff.GetState() == RunnerState.State.Shielded)
            {
                ret = true;
                Destroy(buff);
            }

            return ret;
        }

        #region Character Effects

        public void Kill()
        {
            if (!CheckForShield())
            {
                m_gameplayPlayerManager.OnRunnerDeath(gameObject);
            }
        }

        public void Impulse(Vector3 force)
        {
            if (!CheckForShield()) {
                m_runnerCharacterController.Impulse(force);
            }
        }

        public void Root(int rootHardness)
        {
            if (!CheckForShield())
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
            if (!CheckForShield()) {
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

        public void ActivateWind(float windForce)
        {
            m_wind.AddWindForce(windForce);
        }

        public void DeactivateWind(float windForce)
        {
            m_wind.RemoveWindForce(windForce);
        }

        public void AbsoluteKill()
        {
            m_gameplayPlayerManager.OnRunnerDeath(gameObject);
        }

        #endregion
    }
}