using UnityEngine;

using Run4YourLife.Utils;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public class FullscreenSwitch : MenuEntryArrowedContentText
    {
        private const string ON_FULLSCREEN_STRING = "ON";
        private const string OFF_FULLSCREEN_STRING = "OFF";

        protected override void Awake()
        {
            base.Awake();

            UpdateFullscreenText();
        }

        protected override void OnArrowEvent(MoveEvent moveEvent)
        {
            Screen.fullScreen = moveEvent == MoveEvent.Right;
            StartCoroutine(YieldHelper.SkipFrame(UpdateFullscreenText));
        }

        private void UpdateFullscreenText()
        {
            text.text = Screen.fullScreen ? ON_FULLSCREEN_STRING : OFF_FULLSCREEN_STRING;
        }
    }
}