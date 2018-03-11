using UnityEngine;

namespace Run4YourLife.UI
{
    public class TrapImageUpdater : ImageUpdater
    {
        private enum ActionType
        {
            A, B, X, Y
        }

        [SerializeField]
        private ActionType actionType;

        protected override void Configure()
        {
            switch(actionType)
            {
                case ActionType.A:
                    canDoActionDelegate = laser.CanTrapA;
                    getRemainingTimePercentDelegate = laser.GetTrapARemainingTimePercent;
                    break;
                case ActionType.B:
                    canDoActionDelegate = laser.CanTrapB;
                    getRemainingTimePercentDelegate = laser.GetTrapBRemainingTimePercent;
                    break;
                case ActionType.X:
                    canDoActionDelegate = laser.CanTrapX;
                    getRemainingTimePercentDelegate = laser.GetTrapXRemainingTimePercent;
                    break;
                case ActionType.Y:
                    canDoActionDelegate = laser.CanTrapY;
                    getRemainingTimePercentDelegate = laser.GetTrapYRemainingTimePercent;
                    break;
            }
        }
    }
}