using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    public class PlayerInstance : MonoBehaviour
    {
        public PlayerDefinition playerDefinition;

        private void Start()
        {
            if (playerDefinition == null)
            {
                Debug.LogWarning("Player does not have player definition");
                playerDefinition = CreateDefaultPlayerDefinition();
            }
        }

        private PlayerDefinition CreateDefaultPlayerDefinition()
        {
            return new PlayerDefinition()
            {
                CharacterType = CharacterType.Blue,
                ID = 1,
                IsBoss = false
            };
        }
    }
}

