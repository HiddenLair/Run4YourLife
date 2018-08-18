using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player.Runner

{
    public class RunnerDashBreakableFinder : MonoBehaviour
    {
        [SerializeField]
        private RunnerController m_runnerController;

        private IRunnerDashBreakable m_runnerDashBreakable;

        public IRunnerDashBreakable RunnerDashBreakable { get { return m_runnerDashBreakable; } }

        public void BreakIfAviable()
        {
            if (m_runnerDashBreakable != null && m_runnerDashBreakable.RunnerDashBreakableState == RunnerDashBreakableState.Alive)
            {
                m_runnerDashBreakable.Break();
                m_runnerDashBreakable = null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            IRunnerDashBreakable runnerDashBreakable = other.GetComponent<IRunnerDashBreakable>();
            if (runnerDashBreakable != null && runnerDashBreakable.RunnerDashBreakableState == RunnerDashBreakableState.Alive)
            {
                m_runnerDashBreakable = runnerDashBreakable;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            IRunnerDashBreakable runnerDashBreakable = other.GetComponent<IRunnerDashBreakable>();
            if (runnerDashBreakable != null && runnerDashBreakable == m_runnerDashBreakable)
            {
                m_runnerDashBreakable = null;
            }
        }

        #region Editor
        private void Reset()
        {
            m_runnerController = transform.parent.parent.GetComponent<RunnerController>();
        }

        private void OnValidate()
        {
            Debug.Assert(m_runnerController != null);
        }
        #endregion
    }
}
