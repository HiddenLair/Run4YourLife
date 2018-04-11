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
    class ResolutionSwitch : MonoBehaviour, IMoveHandler, ISelectHandler, IDeselectHandler
    {
        #region Private Variables
        private Resolution[] availableResolutions;
        private int resolutionIndex;
        #endregion

        #region Public Variables
        public TextMeshProUGUI resolutionText;
        public GameObject leftSwitch;
        public GameObject rightSwitch;
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
                    SetResolution(resolutionIndex);
                }

                UpdateUI();
            }
            else if (eventData.moveDir == MoveDirection.Left)
            {
                if (resolutionIndex > 0)
                {
                    resolutionIndex--;
                    SetResolution(resolutionIndex);
                }

                UpdateUI();
            }
        }

        private void SetResolution(int resIndex)
        {
            Screen.SetResolution(availableResolutions[resIndex].width, availableResolutions[resIndex].height, Screen.fullScreen);
        }

        private void UpdateUI()
        {
            resolutionText.text = availableResolutions[resolutionIndex].width + " x " + availableResolutions[resolutionIndex].height;
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (leftSwitch != null)
            {
                leftSwitch.SetActive(true);
            }

            if (rightSwitch != null)
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