using UnityEngine;
using System.Collections.Generic;

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
        FIRST, SECOND, THIRD
    }

    #endregion

    public class UIManager : MonoBehaviour, IUIEvents
    {
        #region Text updaters

        [SerializeField]
        private TextUpdater textUpdaterMele;

        [SerializeField]
        private TextUpdater textUpdaterShoot;

        [SerializeField]
        private TextUpdater textUpdaterTrapA;

        [SerializeField]
        private TextUpdater textUpdaterTrapB;

        [SerializeField]
        private TextUpdater textUpdaterTrapX;

        [SerializeField]
        private TextUpdater textUpdaterTrapY;

        #endregion

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

        #region Progress

        [SerializeField]
        private Progress progress;

        #endregion

        private List<TextUpdater> textUpdaters = new List<TextUpdater>();

        private List<ImageUpdater> imageUpdaters = new List<ImageUpdater>();

        void Awake()
        {
            textUpdaters.Add(textUpdaterMele);
            textUpdaters.Add(textUpdaterShoot);
            textUpdaters.Add(textUpdaterTrapA);
            textUpdaters.Add(textUpdaterTrapB);
            textUpdaters.Add(textUpdaterTrapX);
            textUpdaters.Add(textUpdaterTrapY);

            imageUpdaters.Add(imageUpdaterMele);
            imageUpdaters.Add(imageUpdaterShoot);
            imageUpdaters.Add(imageUpdaterTrapA);
            imageUpdaters.Add(imageUpdaterTrapB);
            imageUpdaters.Add(imageUpdaterTrapX);
            imageUpdaters.Add(imageUpdaterTrapY);
        }

        public void OnActionUsed(ActionType actionType, float time)
        {
            textUpdaters[(int)actionType].Use(time);
            imageUpdaters[(int)actionType].Use(time);
        }

        public void OnSetSetted(SetType setType)
        {

        }

        public void OnPhaseSetted(PhaseType phaseType)
        {
            if(phaseType == PhaseType.SECOND)
            {
                progress.gameObject.SetActive(false);
            }
            else
            {
                progress.gameObject.SetActive(true);
            }

            progress.SetPhase(phaseType);
        }

        public void OnBossProgress(float percent)
        {
            progress.SetPercent(percent);
        }
    }
}