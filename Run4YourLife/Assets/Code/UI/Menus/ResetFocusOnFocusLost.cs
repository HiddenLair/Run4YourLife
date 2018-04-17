using UnityEngine;
using UnityEngine.EventSystems;

public class ResetFocusOnFocusLost : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObjectOnUnableToResetFocus;

    [SerializeField]
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
        Debug.Log(currentSelectedGameObject);

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