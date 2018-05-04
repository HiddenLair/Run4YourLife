using UnityEngine;

using Run4YourLife.SceneManagement;
using UnityEngine.EventSystems;
using Run4YourLife.Input;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    [RequireComponent(typeof(OptionsMenuControlScheme))]
    public class OptionsMenuManager : MonoBehaviour
    {
        [SerializeField]
        private SceneTransitionRequest m_unloadOptionsMenu;

        private OptionsMenuControlScheme m_optionsMenuControlScheme;

        private void Awake()
        {
            m_optionsMenuControlScheme = GetComponent<OptionsMenuControlScheme>();
        }

        private void Start()
        {
            m_optionsMenuControlScheme.InputDevice = InputDeviceManager.Instance.DefaultInputDevice;
            m_optionsMenuControlScheme.Active = true;
            EventSystem.current.firstSelectedGameObject.GetComponent<ISelectHandler>().OnSelect(null);
        }

        private void Update()
        {
            if(m_optionsMenuControlScheme.Exit.Started())
            {
                m_unloadOptionsMenu.Execute();
            }
        }
    }
}
