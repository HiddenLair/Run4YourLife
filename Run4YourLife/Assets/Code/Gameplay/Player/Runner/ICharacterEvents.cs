using UnityEngine;
using UnityEngine.EventSystems;

public interface ICharacterEvents : IEventSystemHandler
{
    void Kill();
    void Impulse(Vector3 direction,float force);
    void Root(int rootHardness);
    void Debuff(StatModifier statsModifier);
    void Burned(int burningTime);
    void ActivateWindLeft();
    void DeactivateWindLeft();
    void ActivateWindRight();
    void DeactivateWindRight();
}
