using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour
{
    #region Private Variables
    private Resolution[] availableResolutions;
    private int resolutionIndex;
    private EventSystem eventSystem;
    #endregion

    #region Public Variables
    public TextMeshProUGUI resolutionText;
    public TextMeshProUGUI fullscreenText;
    public TextMeshProUGUI graphicsText;
    #endregion

    private void Awake()
    {
        eventSystem = EventSystem.current;

        availableResolutions = Screen.resolutions;

        for (int i = 0; i < availableResolutions.Length; ++i)
        {
            if (availableResolutions[i].width == Screen.currentResolution.width &&
               availableResolutions[i].height == Screen.currentResolution.height)
            {
                resolutionIndex = i;
            }
        }
    }

    void Update()
    {
        //Print actual resolution
        resolutionText.text = availableResolutions[resolutionIndex].width + " x " + availableResolutions[resolutionIndex].height;

        //Print actual graphic level
        graphicsText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];

        //Print fullscreen value
        fullscreenText.text = Screen.fullScreen ? "ON" : "OFF"; 
    }
}
