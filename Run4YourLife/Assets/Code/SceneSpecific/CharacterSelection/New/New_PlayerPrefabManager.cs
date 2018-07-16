using System;
using System.Linq;
using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    public class New_PlayerPrefabManager : MonoBehaviour
    {
        [Serializable]
        public class PlayerPrefab
        {
            public bool isBoss;
            public CharacterType characterType;
            public GameObject prefab;
        }

        [SerializeField]
        private PlayerPrefab[] playerPrefabs;

        public GameObject Get(CharacterType characterType, bool isBoss)
        {
            return playerPrefabs.Where((x) => x.characterType == characterType && x.isBoss == isBoss).First().prefab;
        }
    }
}