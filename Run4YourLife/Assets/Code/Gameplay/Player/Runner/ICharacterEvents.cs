using UnityEngine;
using UnityEngine.EventSystems;

public interface ICharacterEvents : IEventSystemHandler
{
    void Kill();
    void AbsoluteKill();
    void Impulse(Vector3 direction,float force);
    void Root(int rootHardness);
    void Debuff(StatModifier statsModifier);
    void Burned(int burningTime);
    void ActivateWind(float windForce);
    void DeactivateWind(float windForce);
}
