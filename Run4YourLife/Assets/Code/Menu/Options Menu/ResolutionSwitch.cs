using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Run4YourLife.OptionsMenu
{
    class ResolutionSwitch : MonoBehaviour, IMoveHandler
    {
        #region Private Variables
        private Resolution[] availableResolutions;
        private int resolutionIndex;
        #endregion

        #region Public Variables
        public TextMeshProUGUI resolutionText;
        #endregion

        public void Awake()
        {
            availableResolutions = Screen.resolutions;

            for (int i = 0; i < availableResolutions.Length; ++i)
            {
                if (availableResolutions[i].width == Screen.currentResolution.width &&
                   availableResolutions[i].height == Screen.currentResolution.height)
                {
                    resolutionIndex = i;
                }
            }

            UpdateUI();
        }

        public void OnMove(AxisEventData eventData)
        {
            if (eventData.moveDir == MoveDirection.Right)
            {
                if (resolutionIndex < availableResolutions.Length-1)
                {
                    resolutionIndex++;
                }

                UpdateUI();
            }
            else if (eventData.moveDir == MoveDirection.Left)
            {
                if (resolutionIndex > 0)
                {
                    resolutionIndex--;
                }

                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            resolutionText.text = availableResolutions[resolutionIndex].width + " x " + availableResolutions[resolutionIndex].height;
        }
    }
}