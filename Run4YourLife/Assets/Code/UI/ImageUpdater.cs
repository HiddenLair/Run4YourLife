using UnityEngine;
using UnityEngine.UI;

namespace Run4YourLife.UI
{
    public class ImageUpdater : Updater
    {
        [SerializeField]
        private Image imageTop = null;

        protected override void OnEnabled()
        {
            imageTop.fillAmount = 0.0f;
        }

        protected override void OnDisabled()
        {
            imageTop.fillAmount = 1.0f;
        }

        protected override void OnCoolDown(float remainingTime, float remainingTimePercent)
        {
            imageTop.fillAmount = remainingTimePercent;
        }
    }
}