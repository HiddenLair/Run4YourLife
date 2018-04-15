using UnityEngine;
using System;


namespace Run4YourLife.Input
{
    public class RunnerInputStated : MonoBehaviour
    {
        #region References

        private RunnerControlScheme m_playerControlScheme;

        #endregion

        #region Member variables

        private float horizontalInput = 0.0f;
        private float verticalInput = 0.0f;
        private bool interactInput = false;
        private bool jumpInput = false;
        private bool rockInput = false;

        #endregion

        private void Awake()
        {
            m_playerControlScheme = GetComponent<RunnerControlScheme>();
        }

        public float GetHorizontalInput()
        {
            return horizontalInput;
        }

        public float GetVerticalInput()
        {
            return verticalInput;
        }

        public bool GetInteractInput()
        {
            return interactInput;
        }

        public bool GetJumpInput()
        {
            return jumpInput;
        }

        public bool GetRockInput()
        {
            return rockInput;
        }

        private void Update()
        {
            Interact();
            Jump();
            HorizontalInput();
            VerticalInput();
            Rock();
        }

        private void Interact()
        {
            interactInput = m_playerControlScheme.interact.Started();

            IInteractInput[] iInteractInputList = GetComponents<IInteractInput>();

            Array.Sort(iInteractInputList, (x, y) => x.GetPriority().CompareTo(y.GetPriority()));
            foreach (IInteractInput iInteractInput in iInteractInputList)
            {
                iInteractInput.ModifyInteractInput(ref interactInput);
            }
        }

        private void Jump()
        {
            jumpInput = m_playerControlScheme.jump.Started();

            IJumpInput[] iJumpInputList = GetComponents<IJumpInput>();

            Array.Sort(iJumpInputList, (x, y) => x.GetPriority().CompareTo(y.GetPriority()));
            foreach (IJumpInput iJumpInput in iJumpInputList)
            {
                iJumpInput.ModifyJumpInput(ref jumpInput);
            }
        }

        private void HorizontalInput()
        {
            horizontalInput = m_playerControlScheme.move.Value();

            IRunnerInput[] iRunnerInputList = GetComponents<IRunnerInput>();

            Array.Sort(iRunnerInputList, (x, y) => x.GetPriority().CompareTo(y.GetPriority()));

            foreach (IRunnerInput iRunnerInput in iRunnerInputList)
            {
                iRunnerInput.ModifyHorizontalInput(ref horizontalInput);
            }
        }

        private void VerticalInput()
        {
            verticalInput = m_playerControlScheme.vertical.Value();

            IVerticalInput[] iVerticalInputList = GetComponents<IVerticalInput>();

            Array.Sort(iVerticalInputList, (x, y) => x.GetPriority().CompareTo(y.GetPriority()));

            foreach (IVerticalInput iVerticalInput in iVerticalInputList)
            {
                iVerticalInput.ModifyVerticalInput(ref verticalInput);
            }
        }

        private void Rock()
        {
            rockInput = m_playerControlScheme.rock.Started();

            IRockInput[] iRockInputList = GetComponents<IRockInput>();

            Array.Sort(iRockInputList, (x, y) => x.GetPriority().CompareTo(y.GetPriority()));

            foreach (IRockInput irockInput in iRockInputList)
            {
                irockInput.ModifyRockInput(ref rockInput);
            }
        }
    }
}
