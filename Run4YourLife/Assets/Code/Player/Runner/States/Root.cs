using UnityEngine;
using Run4YourLife.InputManagement;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(RunnerControlScheme))]
    [RequireComponent(typeof(StatusEffectController))]
    public class Root : MonoBehaviour
    {
        private int m_interactionsUntilRelease;
        private int m_currentInteractionsUntilRelease;

        private StatusEffectSet m_statusEffectSet;

        private RunnerControlScheme m_runnerControlScheme;
        private StatusEffectController m_statusEffectController;

        private void Awake()
        {
            m_runnerControlScheme = GetComponent<RunnerControlScheme>();
            Debug.Assert(m_runnerControlScheme != null);

            m_statusEffectController = GetComponent<StatusEffectController>();
            Debug.Assert(m_statusEffectController != null);
        }

        public void Init(int interactionsUntilRelease, StatusEffectSet statusEffectSet)
        {
            m_interactionsUntilRelease = interactionsUntilRelease;
            m_statusEffectSet = statusEffectSet;

            m_statusEffectController.Add(statusEffectSet);
        }

        private void Update()
        {
            if (m_runnerControlScheme.Dash.Started())
            {
                m_currentInteractionsUntilRelease -= 1;
                if (m_currentInteractionsUntilRelease == 0)
                {
                    m_statusEffectController.Remove(m_statusEffectSet);
                    Destroy(this);
                }
            }
        }

        public void RefreshRoot()
        {
            m_currentInteractionsUntilRelease = m_interactionsUntilRelease;
        }
    }
}