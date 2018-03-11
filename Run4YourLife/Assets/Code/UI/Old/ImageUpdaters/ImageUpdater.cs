/* using UnityEngine;
using UnityEngine.UI;

namespace Run4YourLife.UI
{
    public abstract class ImageUpdater : Updater
    {
        [SerializeField]
        private Image imageTop = null;

        protected GetRemainingTimePercentDelegate getRemainingTimePercentDelegate = null;

        void Update()
        {
            Debug.Assert(canDoActionDelegate != null);
            Debug.Assert(getRemainingTimePercentDelegate != null);

            float remainingTimePercent = getRemainingTimePercentDelegate();

            if(remainingTimePercent == 0.0f && !canDoActionDelegate())
            {
                remainingTimePercent = 1.0f;
            }

            imageTop.fillAmount = remainingTimePercent;
        }

        protected delegate float GetRemainingTimePercentDelegate();
    }
} */