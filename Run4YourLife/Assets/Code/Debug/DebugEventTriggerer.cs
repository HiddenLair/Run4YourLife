using UnityEngine;
using UnityEngine.Events;

public class DebugEventTriggerer : MonoBehaviour {
    public UnityEvent events;
    public KeyCode key;

    public void Update()
    {
        if(Input.GetKeyDown(key))
        {
            events.Invoke();
        }
    }
}
