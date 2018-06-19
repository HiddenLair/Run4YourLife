using UnityEngine;
using UnityEngine.EventSystems;

namespace Run4YourLife.Player
{
    public interface ICharacterEvents : IEventSystemHandler
    {
        void Kill();
        void AbsoluteKill();
        void Impulse(Vector3 force);
        void Shock(float time);
    }
}