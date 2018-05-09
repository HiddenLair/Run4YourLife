using UnityEngine;
using System.Collections.Generic;
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

    public class UIManager : MonoBehaviour, IUIEvents
    {
        #region Image updaters

        [SerializeField]
        private ImageUpdater imageUpdaterMele;

        [SerializeField]
        private ImageUpdater imageUpdaterShoot;

        [SerializeField]
        private ImageUpdater imageUpdaterTrapA;

        [SerializeField]
        private ImageUpdater imageUpdaterTrapB;

        [SerializeField]
        private ImageUpdater imageUpdaterTrapX;

        [SerializeField]
        private ImageUpdater imageUpdaterTrapY;

        #endregion

        #region ScaleTicks

        [SerializeField]
        private ScaleTick scaleTickMele;

        [SerializeField]
        private ScaleTick scaleTickShoot;

        [SerializeField]
        private ScaleTick scaleTickTrapA;

        [SerializeField]
        private ScaleTick scaleTickTrapB;

        [SerializeField]
        private ScaleTick scaleTickTrapX;

        [SerializeField]
        private ScaleTick scaleTickTrapY;

        #endregion

        #region Progress

        [SerializeField]
        private Progress progress;

        #endregion

        #region Countdown

        [SerializeField]
        private Countdown countdown;

        #endregion

        private List<ImageUpdater> imageUpdaters = new List<ImageUpdater>();

        private List<ScaleTick> scaleTicks = new List<ScaleTick>();

        void Awake()
        {
            imageUpdaters.Add(imageUpdaterMele);
            imageUpdaters.Add(imageUpdaterShoot);
            imageUpdaters.Add(imageUpdaterTrapA);
            imageUpdaters.Add(imageUpdaterTrapB);
            imageUpdaters.Add(imageUpdaterTrapX);
            imageUpdaters.Add(imageUpdaterTrapY);

            scaleTicks.Add(scaleTickMele);
            scaleTicks.Add(scaleTickShoot);
            scaleTicks.Add(scaleTickTrapA);
            scaleTicks.Add(scaleTickTrapB);
            scaleTicks.Add(scaleTickTrapX);
            scaleTicks.Add(scaleTickTrapY);

            GameManager.Instance.onGamePhaseChanged.AddListener(ConversorTemporal);
        }

        public void ConversorTemporal(GamePhase phase)//TODO: REFACTOR THIS
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
            OnPhaseSetted(ret);
        }

        public void OnActionUsed(ActionType actionType, float time)
        {
            imageUpdaters[(int)actionType].Use(time);
            scaleTicks[(int)actionType].Tick();
        }

        public void OnSetSetted(SetType setType)
        {

        }

        public void OnPhaseSetted(PhaseType phaseType)
        {
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

        public void OnBossProgress(float percent)
        {
            progress.SetPercent(percent);
        }

        public void OnCountdownSetted(float time)
        {
            countdown.Go(time);
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