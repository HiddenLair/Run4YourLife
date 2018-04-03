using UnityEngine;

public class RunnerState : MonoBehaviour
{
    public enum State
    {
        Burned,
        Root,
        Wind,
        BigHead,
        Shielded
    }

    private State identifier;

    public RunnerState(State id) {
        identifier = id;
    }

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

    public virtual State GetState()
    {
        return identifier;
    }

    public virtual void SetState(State id)
    {
        identifier = id;
    }
}