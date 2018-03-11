using UnityEngine;
using UnityEngine.Events;

public class TriggerEventNotifier : MonoBehaviour {

    public UnityEvent triggerEnter;
    public UnityEvent triggerExit;

    private void OnTriggerEnter(Collider other)
    {
        triggerEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        triggerExit.Invoke();
    }
}