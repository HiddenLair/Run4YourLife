using UnityEngine;
using UnityEngine.EventSystems;

public interface IEventMessageTarget : IEventSystemHandler
{
    void Explosion();
    void Impulse(Vector3 force);
    //void Impulse(Vector3 force);
    //void Burned(int time);
    //void Slow(int time);
}
