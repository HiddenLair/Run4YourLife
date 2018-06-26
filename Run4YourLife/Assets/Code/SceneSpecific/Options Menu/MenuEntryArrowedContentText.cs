using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public abstract class MenuEntryArrowedContentText : MenuEntryArrowed
    {
        [SerializeField]
        protected TextMeshProUGUI text;

        [SerializeField]
        private Color32 colorNormal = new Color32(150, 150, 150, 255);

        [SerializeField]
        private Color32 colorOnFocus = new Color32(255, 255, 255, 255);

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            text.color = colorOnFocus;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            text.color = colorNormal;
        }
    }
}