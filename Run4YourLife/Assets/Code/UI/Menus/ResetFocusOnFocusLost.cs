using UnityEngine;
using UnityEngine.EventSystems;

public class ResetFocusOnFocusLost : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObjectOnUnableToResetFocus;

    private EventSystem eventSystem;
    private GameObject lastGameObjectFocus;

    void Start()
    {
        eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(gameObjectOnUnableToResetFocus);
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