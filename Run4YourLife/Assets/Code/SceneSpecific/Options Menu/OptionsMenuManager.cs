using UnityEngine;

using UnityEngine.EventSystems;
using Run4YourLife.SceneManagement;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public class OptionsMenuManager : MonoBehaviour
    {
        [SerializeField]
        private SceneTransitionRequest m_unloadOptionsMenu;

        private void Start()
        {
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
    }
}