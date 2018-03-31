using UnityEngine;
using System;


namespace Run4YourLife.Input
{
    public class RunnerInputStated : MonoBehaviour
    {

        #region Inputs

        private float horizontalInput = 0.0f;
        private bool interactInput = false;
        private bool jumpInput = false;

        #endregion

        #region Variables

        private RunnerControlScheme m_playerControlScheme;

        #endregion

        private void Awake()
        {
            m_playerControlScheme = GetComponent<RunnerControlScheme>();
        }

        private void Start()
        {
            m_playerControlScheme.Active = true;
        }

        public float GetHorizontalInput()
        {
            return horizontalInput;
        }

        public bool GetInteractInput()
        {
            return interactInput;
        }

        public bool GetJumpInput()
        {
            return jumpInput;
        }

        private void Update()
        {
            #region Interact
            interactInput = m_playerControlScheme.interact.Started();

            IInteractInput[] iInteractInputList = GetComponents<IInteractInput>();

            Array.Sort(iInteractInputList, (x, y) => x.GetPriority().CompareTo(y.GetPriority()));
            foreach (IInteractInput iInteractInput in iInteractInputList)
            {
                iInteractInput.ModifyInteractInput(ref interactInput);
            }
            #endregion
            #region Jump

            jumpInput = m_playerControlScheme.jump.Started();

            IJumpInput[] iJumpInputList = GetComponents<IJumpInput>();

            Array.Sort(iJumpInputList, (x, y) => x.GetPriority().CompareTo(y.GetPriority()));
            foreach (IJumpInput iJumpInput in iJumpInputList)
            {
                iJumpInput.ModifyJumpInput(ref jumpInput);
            }

            #endregion
            #region Horizontal Input

            horizontalInput = m_playerControlScheme.move.Value();

            IRunnerInput[] iRunnerInputList = GetComponents<IRunnerInput>();

            Array.Sort(iRunnerInputList, (x, y) => x.GetPriority().CompareTo(y.GetPriority()));

            foreach (IRunnerInput iRunnerInput in iRunnerInputList)
            {
                iRunnerInput.ModifyHorizontalInput(ref horizontalInput);
            }

            #endregion
        }
    }
}
