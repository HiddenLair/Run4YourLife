using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.SceneManagement;
using Run4YourLife.InputManagement;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.SceneSpecific.TitleScreen
{
    public class TitleScreenManager : MonoBehaviour {

        [SerializeField]
        private SceneTransitionRequest m_mainMenuLoad;

        private void Update()
        {
            if(Input.anyKeyDown)
            {
                m_mainMenuLoad.Execute();
            }
        }
    }
}