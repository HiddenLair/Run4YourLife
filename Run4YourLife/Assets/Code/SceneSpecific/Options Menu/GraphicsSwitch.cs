using UnityEngine;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public class GraphicsSwitch : MenuEntryArrowedContentText
    {
        protected override void Awake()
        {
            base.Awake();

            UpdateGraphicsText();
        }

        protected override void OnArrowEvent(MoveEvent moveEvent)
        {
            switch (moveEvent)
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
            UpdateTextContent(QualitySettings.names[QualitySettings.GetQualityLevel()]);
        }
    }
}