using UnityEngine;

public abstract class RunnerState : MonoBehaviour
{
    public enum Type
    {
        Burned,
        Root,
        Wind,
        BigHead,
        Shielded
    }

    public abstract Type StateType { get; }

    protected virtual void Awake()
    {
        Apply();
    }

    protected virtual void OnDestroy()
    {
        Unapply();
    }

    protected abstract void Apply();

    protected abstract void Unapply();
}