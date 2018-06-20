using System;

using UnityEngine;

using Run4YourLife.GameManagement;

namespace Run4YourLife.UI
{
    #region Useful types for events

    public enum ActionType
    {
        MELE, SHOOT,
        A, B, X, Y
    }

    public enum PhaseType
    {
        FIRST, SECOND, THIRD,
        TRANSITION
    }

    #endregion
    public class UIManager : MonoBehaviour, IUIEvents
    {
        #region  Editor

        [SerializeField]
        private UIBossActionController m_melee;

        [SerializeField]
        private UIBossActionController m_shoot;

        [SerializeField]
        private UIBossActionController m_A;

        [SerializeField]
        private UIBossActionController m_B;

        [SerializeField]
        private UIBossActionController m_X;

        [SerializeField]
        private UIBossActionController m_Y;

        [SerializeField]
        private Progress progress;

        #endregion

        UIBossActionController[] m_actionControllers;

        private void Awake()
        {
            m_actionControllers = new UIBossActionController[Enum.GetNames(typeof(ActionType)).Length];
            m_actionControllers[(int)ActionType.MELE] = m_melee;
            m_actionControllers[(int)ActionType.SHOOT] = m_shoot;
            m_actionControllers[(int)ActionType.A] = m_A;
            m_actionControllers[(int)ActionType.B] = m_B;
            m_actionControllers[(int)ActionType.X] = m_X;
            m_actionControllers[(int)ActionType.Y] = m_Y;

            GameManager.Instance.onGamePhaseChanged.AddListener(OnGamePhaseChanged);
        }

        private void OnGamePhaseChanged(GamePhase gamePhase)
        {
            foreach(UIBossActionController bossActionController in m_actionControllers)
            {
                bossActionController.Reset();
            }

            PhaseType phaseType = GamePhaseConversor(gamePhase);
            if(phaseType == PhaseType.TRANSITION)
            {
                ActiveAll(false);
            }
            else
            {
                ActiveAll(true);
                if (progress != null)
                {
                    progress.gameObject.SetActive(phaseType != PhaseType.SECOND);
                }
            }
            if (progress != null)
            {
                progress.SetPhase(phaseType);
            }
        }

        public void OnActionUsed(ActionType actionType, float cooldown)
        {
            m_actionControllers[(int)actionType].Use(cooldown);
        }

        public void OnBossProgress(float percent)
        {
            if (progress != null)
            {
                progress.SetPercent(percent);
            }
        }

        private PhaseType GamePhaseConversor(GamePhase phase)
        {
            PhaseType ret = PhaseType.TRANSITION;
            switch (phase)
            {
                case GamePhase.EasyMoveHorizontal:
                    {
                        ret = PhaseType.FIRST;
                    }
                    break;
                case GamePhase.BossFight:
                    {
                        ret = PhaseType.SECOND;
                    }
                    break;
                case GamePhase.HardMoveHorizontal:
                    {
                        ret = PhaseType.THIRD;
                    }
                    break;
            }
            return ret;
        }

        private void ActiveAll(bool active)
        {
            for(int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).gameObject.SetActive(active);
            }
        }
    }
}

