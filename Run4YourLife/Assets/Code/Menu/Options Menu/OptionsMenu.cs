using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    #region Private Variables
    private Resolution[] availableResolutions;
    #endregion

    #region Public Variables
    #endregion

    void Start ()
    {
        availableResolutions = Screen.resolutions;
	}
	
}
