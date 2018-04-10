using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Run4YourLife.OptionsMenu
{
    class FullscreenSwitch : MonoBehaviour, IMoveHandler
    {
        #region Public Variables
        public TextMeshProUGUI fullscreenText;
        #endregion

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

        private void UpdateUI()
        {
            fullscreenText.text = Screen.fullScreen ? "ON" : "OFF";
        }
    }
}