using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Input;

namespace Run4YourLife.Player
{
    public class PlayerInstance : MonoBehaviour, IPlayerDefinitionEvents
    {

        private PlayerDefinition m_playerDefinition;

        public PlayerDefinition PlayerDefinition {
            get {
                return m_playerDefinition;
            }

            private set {
                m_playerDefinition = value;
            }
        }

        void Start()
        {
            if (m_playerDefinition == null)
            {
                Debug.LogWarning("Player does not have player definition, creating an instance using the default properties");
                PlayerDefinition defaultPlayerDefinition = CreateDefaultPlayerDefinition();
                ExecuteEvents.Execute<IPlayerDefinitionEvents>(gameObject, null, (a, b) => a.OnPlayerDefinitionChanged(defaultPlayerDefinition));
            }
        }

        public void OnPlayerDefinitionChanged(PlayerDefinition playerDefinition)
        {
            PlayerDefinition = playerDefinition;
        }

        private PlayerDefinition CreateDefaultPlayerDefinition()
        {
            return new PlayerDefinition()
            {
                CharacterType = CharacterType.Blue,
                ID = 1,
                IsBoss = false,
                inputDevice = new InputDevice(1)
            };
        }
    }
}

