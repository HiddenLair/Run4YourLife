using UnityEngine;

public class RunnerState : MonoBehaviour
{
    void Awake()
    {
        Apply();
    }

    void OnDestroy()
    {
        Unapply();
    }

    protected virtual void Apply() { }

    protected virtual void Unapply() { }
}