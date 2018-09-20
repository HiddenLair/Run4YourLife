using UnityEngine;
using System.Collections.Generic;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public class ResolutionSwitch : MenuEntryArrowedContentText
    {
        private int resolutionIndex = -1;
        private List<Resolution> availableResolutions = new List<Resolution>();

        protected override void Awake()
        {
            base.Awake();

            BuildResolutionsList();
            FindInitialResolutionIndex();
            UpdateResolutionText();
        }

        protected override void OnArrowEvent(MoveEvent moveEvent)
        {
            resolutionIndex += 2 * (int)moveEvent - 1;
            resolutionIndex = Mathf.Clamp(resolutionIndex, 0, availableResolutions.Count - 1);

            UpdateResolution();
        }

        private void BuildResolutionsList()
        {
            // Find all available resolutions while ignoring their refresh rate
            // They are sorted by their width and height

            Resolution[] resolutions = Screen.resolutions;

            availableResolutions.Add(resolutions[0]);

            foreach (Resolution resolution in resolutions)
            {
                Resolution lastResolutionAdded = availableResolutions[availableResolutions.Count - 1];

                if (resolution.width != lastResolutionAdded.width || resolution.height != lastResolutionAdded.height)
                {
                    availableResolutions.Add(resolution);
                }
            }
        }

        private void FindInitialResolutionIndex()
        {
            resolutionIndex = availableResolutions.Count - 1;

            for (int i = 0; i < availableResolutions.Count; ++i)
            {
                if (availableResolutions[i].width == Screen.width && availableResolutions[i].height == Screen.height)
                {
                    resolutionIndex = i;
                    break;
                }
            }
        }

        private void UpdateResolution()
        {
            Screen.SetResolution(availableResolutions[resolutionIndex].width, availableResolutions[resolutionIndex].height, Screen.fullScreen);
            UpdateResolutionText();
        }

        private void UpdateResolutionText()
        {
            UpdateTextContent(availableResolutions[resolutionIndex].width + " x " + availableResolutions[resolutionIndex].height);
        }
    }
}