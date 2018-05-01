using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public class  FullscreenSwitch : MonoBehaviour, IMoveHandler, ISelectHandler, IDeselectHandler
    {
        #region Public Variables
        [SerializeField]
        private TextMeshProUGUI fullscreenText;

        [SerializeField]
        private GameObject leftSwitch;

        [SerializeField]
        private GameObject rightSwitch;
        #endregion

        private void UpdateUI()
        {
            fullscreenText.text = Screen.fullScreen ? "ON" : "OFF";
        }

        public void OnMove(AxisEventData eventData)
        {
            if (eventData.moveDir == MoveDirection.Right)
            {
                Screen.fullScreen = true;
                UpdateUI();

                rightSwitch.GetComponent<ScaleTick>().Tick();
            }
            else if (eventData.moveDir == MoveDirection.Left)
            {
                Screen.fullScreen = false;
                UpdateUI();

                leftSwitch.GetComponent<ScaleTick>().Tick();
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            if(leftSwitch != null)
            {
                leftSwitch.SetActive(true);
            }

            if(rightSwitch != null)
            {
                rightSwitch.SetActive(true);
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (leftSwitch != null)
            {
                leftSwitch.SetActive(false);
            }

            if (rightSwitch != null)
            {
                rightSwitch.SetActive(false);
            }
        }
    }
}