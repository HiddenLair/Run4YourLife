using UnityEngine;
using UnityEngine.Events;

using Run4YourLife.Debugging;

public class DebugEventTriggerer : MonoBehaviour {
    public UnityEvent events;
    public KeyCode key;

    private void Awake()
    {
        enabled = DebugSystemManagerHelper.DebuggingToolsEnabled();
    }

    public void Update()
    {
        if(Input.GetKeyDown(key))
        {
            events.Invoke();
        }
    }
}
