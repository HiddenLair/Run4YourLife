using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{

	// Use this for initialization
    public void onPausePressed()
    {
        Time.timeScale = 0;
    }
}
