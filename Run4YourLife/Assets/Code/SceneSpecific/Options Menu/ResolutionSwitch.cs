using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public class ResolutionSwitch : MenuEntryArrowed
    {
        [SerializeField]
        private TextMeshProUGUI resolutionText;

        private Resolution[] availableResolutions;
        private int resolutionIndex;        

        protected override void Awake()
        {
            base.Awake();
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

        private void SetResolution(int resIndex)
        {
            Screen.SetResolution(availableResolutions[resIndex].width, availableResolutions[resIndex].height, Screen.fullScreen);
        }

        private void UpdateUI()
        {
            resolutionText.text = availableResolutions[resolutionIndex].width + " x " + availableResolutions[resolutionIndex].height;
        }

        protected override void OnArrowEvent(MoveEvent moveEvent)
        {
            switch(moveEvent)
            {
                case MoveEvent.Left:
                    if (resolutionIndex > 0)
                    {
                        resolutionIndex--;
                        SetResolution(resolutionIndex);
                    }
                    break;
                case MoveEvent.Right:
                    if (resolutionIndex < availableResolutions.Length-1)
                    {
                        resolutionIndex++;
                        SetResolution(resolutionIndex);
                    }
                    break;
            }
            UpdateUI();
        }
    }
}