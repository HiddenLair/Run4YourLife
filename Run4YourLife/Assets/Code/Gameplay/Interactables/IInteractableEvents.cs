using UnityEngine;
using UnityEngine.EventSystems;

public interface IInteractableEvents : IEventSystemHandler
{
    void Interact();
}
