using UnityEngine;
using System.Collections.Generic;
using Run4YourLife.GameManagement;

namespace Run4YourLife.UI
{


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
        private ScaleTick scaleTickMeleBottom;

        [SerializeField]
        private ScaleTick scaleTickMeleTop;

        [SerializeField]
        private ScaleTick scaleTickShootBottom;

        [SerializeField]
        private ScaleTick scaleTickShootTop;

        [SerializeField]
        private ScaleTick scaleTickTrapABottom;

        [SerializeField]
        private ScaleTick scaleTickTrapATop;

        [SerializeField]
        private ScaleTick scaleTickTrapBBottom;

        [SerializeField]
        private ScaleTick scaleTickTrapBTop;

        [SerializeField]
        private ScaleTick scaleTickTrapXBottom;

        [SerializeField]
        private ScaleTick scaleTickTrapXTop;

        [SerializeField]
        private ScaleTick scaleTickTrapYBottom;

        [SerializeField]
        private ScaleTick scaleTickTrapYTop;

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

        private List<ScaleTick> scaleTicksBottom = new List<ScaleTick>();
        private List<ScaleTick> scaleTicksTop = new List<ScaleTick>();

        void Awake()
        {
            imageUpdaters.Add(imageUpdaterMele);
            imageUpdaters.Add(imageUpdaterShoot);
            imageUpdaters.Add(imageUpdaterTrapA);
            imageUpdaters.Add(imageUpdaterTrapB);
            imageUpdaters.Add(imageUpdaterTrapX);
            imageUpdaters.Add(imageUpdaterTrapY);

            scaleTicksBottom.Add(scaleTickMeleBottom);
            scaleTicksBottom.Add(scaleTickShootBottom);
            scaleTicksBottom.Add(scaleTickTrapABottom);
            scaleTicksBottom.Add(scaleTickTrapBBottom);
            scaleTicksBottom.Add(scaleTickTrapXBottom);
            scaleTicksBottom.Add(scaleTickTrapYBottom);

            scaleTicksTop.Add(scaleTickMeleTop);
            scaleTicksTop.Add(scaleTickShootTop);
            scaleTicksTop.Add(scaleTickTrapATop);
            scaleTicksTop.Add(scaleTickTrapBTop);
            scaleTicksTop.Add(scaleTickTrapXTop);
            scaleTicksTop.Add(scaleTickTrapYTop);

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
            scaleTicksBottom[(int)actionType].Tick();
            scaleTicksTop[(int)actionType].Tick();
        }

        public void OnSetSetted(SetType setType)
        {
            throw new System.NotImplementedException("Not Implemented");
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