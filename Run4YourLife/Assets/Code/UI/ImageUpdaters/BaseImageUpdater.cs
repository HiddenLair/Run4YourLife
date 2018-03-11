using UnityEngine;

namespace Run4YourLife.UI
{
    public class BaseImageUpdater : ImageUpdater
    {
        private enum ActionType
        {
            MELE, SHOOT
        }

        [SerializeField]
        private ActionType actionType;

        protected override void Configure()
        {
            switch(actionType)
            {
                case ActionType.MELE:
                    canDoActionDelegate = boss.CanMele;
                    getRemainingTimePercentDelegate = boss.GetMeleRemainingTimePercent;
                    break;
                case ActionType.SHOOT:
                    canDoActionDelegate = boss.CanShoot;
                    getRemainingTimePercentDelegate = boss.GetShootRemainingTimePercent;
                    break;
            }
        }
    }
}