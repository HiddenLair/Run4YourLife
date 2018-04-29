using UnityEngine;
using UnityEngine.EventSystems;

namespace Run4YourLife.Player
{
    public interface ICharacterEvents : IEventSystemHandler
    {
        void Kill();
        void AbsoluteKill();
        void Impulse(Vector3 force);
        void Root(int rootHardness);
        void Debuff(AttributeModifier statsModifier);
        void Burned(int burningTime);
        void ActivateWind(float windForce, ref Wind component);
        void DeactivateWind(float windForce, Wind component);
    }
}