using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public class GraphicsSwitch : MenuEntryArrowed
    {
        public TextMeshProUGUI m_graphicsText;

        protected override void Awake()
        {
            base.Awake();
            UpdateUI();
        }

        private void UpdateUI()
        {
            m_graphicsText.SetText(QualitySettings.names[QualitySettings.GetQualityLevel()]);
        }

        protected override void OnArrowEvent(MoveEvent moveEvent)
        {
            switch(moveEvent)
            {
                case MoveEvent.Left:
                    QualitySettings.DecreaseLevel();
                    break;
                case MoveEvent.Right:
                    QualitySettings.IncreaseLevel();
                    break;
            }
            UpdateUI();
        }
    }
}
