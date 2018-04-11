using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Run4YourLife.OptionsMenu
{
    class FullscreenSwitch : MonoBehaviour, IMoveHandler, ISelectHandler, IDeselectHandler
    {
        #region Public Variables
        public TextMeshProUGUI fullscreenText;
        public GameObject leftSwitch;
        public GameObject rightSwitch;
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
            }
            else if (eventData.moveDir == MoveDirection.Left)
            {
                Screen.fullScreen = false;
                UpdateUI();
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