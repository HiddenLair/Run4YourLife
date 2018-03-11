using UnityEngine.UI;

namespace Run4YourLife.UI
{
    public class TextUpdater : Updater
    {
        private Text text;

        void Start()
        {
            text = GetComponent<Text>();
        }

        protected override void OnEnabled()
        {
            text.text = "ENABLED";
        }

        protected override void OnDisabled()
        {
            text.text = "DISABLED";
        }

        protected override void OnCoolDown(float remainingTime, float remainingTimePercent)
        {
            text.text = remainingTime.ToString("0.###");
        }
    }
}