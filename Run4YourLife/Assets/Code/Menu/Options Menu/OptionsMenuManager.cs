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
        private SceneTransitionRequest m_unloadOptionsMenu;

        private ExitButtonControlScheme exitScheme;

        private void Awake()
        {
            exitScheme = GetComponent<ExitButtonControlScheme>();
        }

        private void Start()
        {
            exitScheme.InputDevice = new InputDevice(0); //TODO use inputDeviceManager.DefaultInputDevice instead
            exitScheme.Active = true;
            EventSystem.current.firstSelectedGameObject.GetComponent<ISelectHandler>().OnSelect(null);
        }

        private void Update()
        {
            if(exitScheme.exitAction.Started())
            {
                m_unloadOptionsMenu.Execute();
            }
        }
    }
}
