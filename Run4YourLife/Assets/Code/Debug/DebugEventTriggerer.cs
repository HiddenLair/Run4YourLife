using UnityEngine;
using UnityEngine.Events;

public class DebugEventTriggerer : MonoBehaviour {
    public UnityEvent events;
    public KeyCode key;

    private void Awake()
    {
        enabled = Debug.isDebugBuild;
    }

    public void Update()
    {
        if(Input.GetKeyDown(key))
        {
            events.Invoke();
        }
    }
}
