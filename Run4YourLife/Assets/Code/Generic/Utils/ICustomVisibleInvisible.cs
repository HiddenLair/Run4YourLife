using UnityEngine.EventSystems;

namespace Run4YourLife.Player
{
    public interface ICustomVisibleInvisible : IEventSystemHandler
    {
        void OnCustomBecameInvisible();
        void OnCustomBecameVisible();
    }
}