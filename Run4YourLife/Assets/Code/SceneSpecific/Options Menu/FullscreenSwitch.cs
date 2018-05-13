using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

using TMPro;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public class  FullscreenSwitch : MenuEntryArrowed
    {
        [SerializeField]
        private TextMeshProUGUI m_fullscreenText;

        private string m_onString = "ON";
        private string m_offString = "OFF";

        protected override void OnArrowEvent(MoveEvent moveEvent)
        {
            switch(moveEvent)
            {
                case MoveEvent.Right:
                    Screen.fullScreen = true;
                    m_fullscreenText.text = m_onString;
                    break;

                case MoveEvent.Left:
                    Screen.fullScreen = false;
                    m_fullscreenText.text = m_offString;
                    break;
            }
        }
    }
}