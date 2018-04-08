using UnityEngine;
using UnityEngine.EventSystems;

public class ResetFocusOnFocusLost : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObjectOnUnableToResetFocus;

    private EventSystem eventSystem;
    private GameObject lastGameObjectFocus;

    void Awake()
    {
        eventSystem = EventSystem.current;
    }

    void Update()
    {
        GameObject currentSelectedGameObject = eventSystem.currentSelectedGameObject;

        if(currentSelectedGameObject != null)
        {
            lastGameObjectFocus = currentSelectedGameObject;
        }
        else
        {
            if(lastGameObjectFocus == null)
            {
                lastGameObjectFocus = gameObjectOnUnableToResetFocus;
            }

            eventSystem.SetSelectedGameObject(lastGameObjectFocus);
        }
    }
}