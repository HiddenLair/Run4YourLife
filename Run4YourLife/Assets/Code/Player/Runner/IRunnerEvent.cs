using UnityEngine.EventSystems;

public interface IRunnerEvent : IEventSystemHandler
{
    void OnPlayerHasBeenJumpedOnTop();
    void OnPlayerJumpedOnTop();
}
