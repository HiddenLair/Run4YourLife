using UnityEngine;

namespace Run4YourLife.UI
{
    public class BaseTextUpdater : TextUpdater
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
                    getRemainingTimeDelegate = boss.GetMeleRemainingTime;
                    break;
                case ActionType.SHOOT:
                    canDoActionDelegate = boss.CanShoot;
                    getRemainingTimeDelegate = boss.GetShootRemainingTime;
                    break;
            }
        }
    }
}