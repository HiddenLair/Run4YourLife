using UnityEngine;

using Run4YourLife.SceneManagement;
using UnityEngine.EventSystems;
using Run4YourLife.InputManagement;

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

            GameObject firstSelectedGameObject = EventSystem.current.firstSelectedGameObject;
            if(firstSelectedGameObject != null)
            {
                ISelectHandler firstSelected = firstSelectedGameObject.GetComponent<ISelectHandler>();
                if(firstSelected != null)
                {
                    firstSelected.OnSelect(null);
                }
            }
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
