using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.Input;

namespace Run4YourLife.Player
{
    public class PlayerInstance : MonoBehaviour
    {

        private PlayerDefinition m_playerDefinition;

        public PlayerDefinition PlayerDefinition {
            get {
                if (m_playerDefinition == null)
                {
                    Debug.LogWarning("Player does not have player definition, creating an instance using the default properties");
                    m_playerDefinition = CreateDefaultPlayerDefinition();
                }
                return m_playerDefinition;
            }

            set {
                m_playerDefinition = value;
            }
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

