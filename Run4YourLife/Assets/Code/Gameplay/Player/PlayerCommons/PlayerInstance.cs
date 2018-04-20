using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Input;

namespace Run4YourLife.Player
{
    public class PlayerInstance : MonoBehaviour, IPlayerHandleEvent
    {
        public PlayerHandle PlayerHandle { get; private set; }

        void Start()
        {
            if (PlayerHandle == null)
            {
                Debug.LogWarning("Player does not have player definition, creating an instance using the default properties");
                PlayerHandle defaultPlayerDefinition = PlayerHandle.DebugDefaultPlayerHandle;
                ExecuteEvents.Execute<IPlayerHandleEvent>(gameObject, null, (a, b) => a.OnPlayerDefinitionChanged(defaultPlayerDefinition));
            }
        }

        public void OnPlayerDefinitionChanged(PlayerHandle playerDefinition)
        {
            PlayerHandle = playerDefinition;
        }
    }
}

