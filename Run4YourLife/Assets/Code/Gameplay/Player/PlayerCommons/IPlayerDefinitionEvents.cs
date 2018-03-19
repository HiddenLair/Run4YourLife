using UnityEngine.EventSystems;

namespace Run4YourLife.Player
{ 
    public interface IPlayerDefinitionEvents : IEventSystemHandler
    {
        void OnPlayerDefinitionChanged(PlayerDefinition playerDefinition);
    }
}
