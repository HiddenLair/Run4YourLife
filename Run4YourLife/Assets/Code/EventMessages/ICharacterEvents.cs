using UnityEngine;
using UnityEngine.EventSystems;

public interface ICharacterEvents : IEventSystemHandler
{
    void Explosion();
    void Impulse(Vector3 force);
    void Root(int rootHardness);
    void Debuff(StatModifier statsModifier);
    void Burned(int burningTime);
    void ActivateWindPush();
    void DeactivateWindPush();
}
