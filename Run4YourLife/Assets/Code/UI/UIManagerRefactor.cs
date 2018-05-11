using System;

using UnityEngine;

using Run4YourLife.GameManagement;

namespace Run4YourLife.UI
{
    #region Useful types for events

    public enum ActionType
    {
        MELE, SHOOT,
        TRAP_A, TRAP_B, TRAP_X, TRAP_Y,
        SKILL_A, SKILL_B, SKILL_X, SKILL_Y
    }

    public enum SetType
    {
        TRAPS, SKILLS
    }

    public enum PhaseType
    {
        FIRST, SECOND, THIRD,
        TRANSITION
    }

    #endregion
    public class UIManagerRefactor : MonoBehaviour, IUIEvents
    {
        #region  Editor

        [SerializeField]
        private UIBossActionController m_melee;
        [SerializeField]
        private UIBossActionController m_shoot;

        [SerializeField]
        private UIBossActionController m_trapA;

        [SerializeField]
        private UIBossActionController m_trapB;

        [SerializeField]
        private UIBossActionController m_trapX;

        [SerializeField]
        private UIBossActionController m_trapY;

        [SerializeField]
        private UIBossActionController m_skillA;

        [SerializeField]
        private UIBossActionController m_skillB;

        [SerializeField]
        private UIBossActionController m_skillX;

        [SerializeField]
        private UIBossActionController m_skillY;

        [SerializeField]
        private Progress progress;

        [SerializeField]
        private Countdown countdown;

        #endregion

        UIBossActionController[] m_actionControllers;

        private void Awake()
        {
            
            m_actionControllers = new  UIBossActionController[Enum.GetNames(typeof(ActionType)).Length];
            m_actionControllers[(int)ActionType.MELE] = m_melee;
            m_actionControllers[(int)ActionType.SHOOT] = m_shoot;
            m_actionControllers[(int)ActionType.TRAP_A] = m_trapA;
            m_actionControllers[(int)ActionType.TRAP_B] = m_trapB;
            m_actionControllers[(int)ActionType.TRAP_X] = m_trapX;
            m_actionControllers[(int)ActionType.TRAP_Y] = m_trapY;
            m_actionControllers[(int)ActionType.SKILL_A] = m_skillA;
            m_actionControllers[(int)ActionType.SKILL_B] = m_skillB;
            m_actionControllers[(int)ActionType.SKILL_X] = m_skillX;
            m_actionControllers[(int)ActionType.SKILL_Y] = m_skillY;

            GameManager.Instance.onGamePhaseChanged.AddListener(OnGamePhaseChanged);
        }

        private void OnGamePhaseChanged(GamePhase gamePhase)
        {
            PhaseType phaseType = GamePhaseConversor(gamePhase);
            if(phaseType == PhaseType.TRANSITION)
            {
                ActiveAll(false);
            }
            else
            {
                ActiveAll(true);
                progress.gameObject.SetActive(phaseType != PhaseType.SECOND);
                countdown.gameObject.SetActive(phaseType == PhaseType.SECOND);
            }

            progress.SetPhase(phaseType);
        }

        public void OnActionUsed(ActionType actionType, float cooldown)
        {
            m_actionControllers[(int)actionType].Use(cooldown);
        }

        public void OnBossProgress(float percent)
        {
            progress.SetPercent(percent);
        }

        public void OnCountdownSetted(float time)
        {
            countdown.Go(time);
        }

        public void OnSetSetted(SetType setType)
        {
            throw new System.NotImplementedException("Not Implemented");
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

