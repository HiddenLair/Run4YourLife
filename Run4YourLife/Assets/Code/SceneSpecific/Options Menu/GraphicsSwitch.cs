using TMPro;
using UnityEngine;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public class GraphicsSwitch : MenuEntryArrowed
    {
        [SerializeField]
        private TextMeshProUGUI m_graphicsText;

        protected override void Awake()
        {
            base.Awake();

            UpdateGraphicsText();
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

            UpdateGraphicsText();
        }

        private void UpdateGraphicsText()
        {
            m_graphicsText.SetText(QualitySettings.names[QualitySettings.GetQualityLevel()]);
        }
    }
}