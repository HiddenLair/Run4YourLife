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
    [RequireComponent(typeof(RunnerControlScheme))]
    [RequireComponent(typeof(RunnerCharacterController))]
    [RequireComponent(typeof(Stats))]
    [RequireComponent(typeof(Animator))]
    public class RunnerController : MonoBehaviour, ICharacterEvents
    {
        #region References

        private Stats m_stats;
        private RunnerCharacterController m_runnerCharacterController;
        private Animator m_animator;
        private RunnerInputStated inputPlayer;

        #endregion

        private void Awake()
        {
            m_runnerCharacterController = GetComponent<RunnerCharacterController>();
            m_stats = GetComponent<Stats>();
            m_animator = GetComponent<Animator>();
            inputPlayer = GetComponent<RunnerInputStated>();
        }

        private void OnTriggerStay(Collider collider)
        {
            if (collider.CompareTag(Tags.Interactable) && inputPlayer.GetInteractInput())
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
                GameObject playerStateManager = FindObjectOfType<GameplayPlayerManager>().gameObject;
                ExecuteEvents.Execute<IGameplayPlayerEvents>(playerStateManager, null, (x, y) => x.OnRunnerDeath(gameObject));
            }
        }

        public void Impulse(Vector3 direction, float force)
        {
            if (!CheckForShield()) {
                m_runnerCharacterController.Impulse(direction, force);
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
            GetComponent<Wind>().AddWindForce(windForce);
        }

        public void DeactivateWind(float windForce)
        {
            GetComponent<Wind>().RemoveWindForce(windForce);
        }

        public void AbsoluteKill()
        {
            GameObject playerStateManager = FindObjectOfType<GameplayPlayerManager>().gameObject;
            ExecuteEvents.Execute<IGameplayPlayerEvents>(playerStateManager, null, (x, y) => x.OnRunnerDeath(gameObject));
        }

        #endregion
    }
}