using UnityEngine;
using System.Collections.Generic;

namespace Run4YourLife.UI
{
    public enum ActionType
    {
        MELE, SHOOT,
        TRAP_A, TRAP_B, TRAP_X, TRAP_Y,
        SKILL_A, SKILL_B, SKILL_X, SKILL_Y
    }

    public enum ActionGroupType
    {
        BASE,
        TRAP,
        SKILL
    }

    public class UIManager : MonoBehaviour, IUIEvents
    {
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

            /* switch(actionType)
            {
                case ActionType.MELE:
                    textUpdaterMele.Use(time);
                    imageUpdaterMele.Use(time);
                    break;
                case ActionType.SHOOT:
                    textUpdaterShoot.Use(time);
                    imageUpdaterShoot.Use(time);
                    break;
                case ActionType.TRAP_A:
                    textUpdaterTrapA.Use(time);
                    imageUpdaterTrapA.Use(time);
                    break;
                case ActionType.TRAP_B:
                    textUpdaterTrapB.Use(time);
                    imageUpdaterTrapB.Use(time);
                    break;
                case ActionType.TRAP_X:
                    textUpdaterTrapX.Use(time);
                    imageUpdaterTrapX.Use(time);
                    break;
                case ActionType.TRAP_Y:
                    textUpdaterTrapY.Use(time);
                    imageUpdaterTrapY.Use(time);
                    break;
            } */
        }

        public void OnEnableActions(bool enabled)
        {
            foreach(TextUpdater textUpdater in textUpdaters)
            {
                textUpdater.Enable(enabled);
            }

            foreach(ImageUpdater imageUpdater in imageUpdaters)
            {
                imageUpdater.Enable(enabled);
            }

            /* switch(actionGroupType)
            {
                case ActionGroupType.BASE:
                    textUpdaterMele.Disable();
                    imageUpdaterMele.Disable();
                    textUpdaterShoot.Disable();
                    imageUpdaterShoot.Disable();
                    break;
                case ActionGroupType.TRAP:
                    textUpdaterTrapA.Disable();
                    imageUpdaterTrapA.Disable();
                    textUpdaterTrapB.Disable();
                    imageUpdaterTrapB.Disable();
                    textUpdaterTrapX.Disable();
                    imageUpdaterTrapX.Disable();
                    textUpdaterTrapY.Disable();
                    imageUpdaterTrapY.Disable();
                    break;
                case ActionGroupType.SKILL:
                    break;
            } */
        }
    }
}