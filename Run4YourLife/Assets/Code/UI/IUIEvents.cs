using UnityEngine.EventSystems;

namespace Run4YourLife.UI
{
    public interface IUIEvents : IEventSystemHandler
    {
        void OnActionUsed(ActionType actionType, float time);

        void OnBossProgress(float percent);
    }
}