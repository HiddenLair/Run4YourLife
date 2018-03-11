using UnityEngine;

namespace Run4YourLife.UI
{
    public class TrapTextUpdater : TextUpdater
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
                    getRemainingTimeDelegate = laser.GetTrapARemainingTime;
                    break;
                case ActionType.B:
                    canDoActionDelegate = laser.CanTrapB;
                    getRemainingTimeDelegate = laser.GetTrapBRemainingTime;
                    break;
                case ActionType.X:
                    canDoActionDelegate = laser.CanTrapX;
                    getRemainingTimeDelegate = laser.GetTrapXRemainingTime;
                    break;
                case ActionType.Y:
                    canDoActionDelegate = laser.CanTrapY;
                    getRemainingTimeDelegate = laser.GetTrapYRemainingTime;
                    break;
            }
        }
    }
}