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
        private RunnerControlScheme m_playerControlScheme;
        private RunnerCharacterController m_runnerCharacterController;
        private Animator m_animator;

        #endregion

        #region Input Variables

        bool interactInput = false;

        #endregion

        private void Awake()
        {
            m_playerControlScheme = GetComponent<RunnerControlScheme>();
            m_runnerCharacterController = GetComponent<RunnerCharacterController>();
            m_stats = GetComponent<Stats>();
            m_animator = GetComponent<Animator>();
        }

        private void Start()
        {
            m_playerControlScheme.Active = true;
        }

        private void OnTriggerStay(Collider collider)
        {
            if (collider.CompareTag(Tags.Interactable) && interactInput)
            {
                Interact(collider.gameObject);
            }
        }

        private void Update()
        {           
            interactInput = m_playerControlScheme.interact.Started();

            IInteractInput[] iInteractInputList = GetComponents<IInteractInput>();

            Array.Sort(iInteractInputList, (x, y) => x.GetPriority().CompareTo(y.GetPriority()));
            foreach (IInteractInput iInteractInput in iInteractInputList)
            {
                iInteractInput.ModifyInteractInput(ref interactInput);
            }
        }

        private void Interact(GameObject gameObject)
        {
            ExecuteEvents.Execute<IInteractableEvents>(gameObject, null, (x, y) => x.Interact());
        }

        public void Kill()
        {
            GameObject playerStateManager = FindObjectOfType<GameplayPlayerManager>().gameObject;
            ExecuteEvents.Execute<IGameplayPlayerEvents>(playerStateManager, null, (x, y) => x.OnRunnerDeath(gameObject));
        }

        public void Impulse(Vector3 direction, float force)
        {
            m_runnerCharacterController.Impulse(direction, force);
        }

        public void Root(int rootHardness)
        {
            Root oldInstance = gameObject.GetComponent<Root>();
            if (oldInstance != null)
            {
                Destroy(oldInstance);
            }
            gameObject.AddComponent<Root>();
        }//TODO, shall we use rootHardness?


        public void Debuff(StatModifier statmodifier)
        {
            m_stats.AddModifier(statmodifier);
        }

        public void Burned(int burnedTime)
        {
            Burned oldInstance = gameObject.GetComponent<Burned>();
            if (oldInstance != null)
            {
                Destroy(oldInstance);
            }
            gameObject.AddComponent<Burned>();
        }//TODO, shall we use burnedTime?

        #region WindLeft

        public void ActivateWindLeft()
        {
            gameObject.AddComponent<WindLeft>();
        }

        public void DeactivateWindLeft()
        {
            Destroy(gameObject.GetComponent<WindLeft>());
        }

        #endregion

        #region WindRight

        public void ActivateWindRight()
        {
            gameObject.AddComponent<WindRight>();
        }

        public void DeactivateWindRight()
        {
            Destroy(gameObject.GetComponent<WindRight>());
        }

        #endregion
    }
}