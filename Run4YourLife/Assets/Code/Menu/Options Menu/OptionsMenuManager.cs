using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Run4YourLife.SceneManagement;
using UnityEngine.EventSystems;
using Run4YourLife.Input;

namespace Run4YourLife.OptionsMenu
{
    [RequireComponent(typeof(ExitButtonControlScheme))]
    public class OptionsMenuManager : MonoBehaviour
    {
        [SerializeField]
        private SceneLoadRequest m_mainMenuLoadRequest;

        private ExitButtonControlScheme exitScheme;

        private bool exitRequest = false;

        private void Awake()
        {
            exitScheme = GetComponent<ExitButtonControlScheme>();
        }

        private void Start()
        {
            exitScheme.InputDevice = new InputDevice(1);
            exitScheme.Active = true;
            EventSystem.current.firstSelectedGameObject.GetComponent<ISelectHandler>().OnSelect(null);
        }

        private void Update()
        {
            exitRequest = exitScheme.exitAction.Started();

            if(exitRequest)
            {
                m_mainMenuLoadRequest.Execute();
            }
        }
    }
}
