using UnityEngine;
using UnityEngine.EventSystems;

public interface IPropEvents : IEventSystemHandler
{
    void OnInteraction();
}
