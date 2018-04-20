using System;
using UnityEngine;
using System.Linq;

namespace Run4YourLife.Player
{
    public class RunnerPrefabManager : MonoBehaviour
    {
        [Serializable]
        public class RunnerPrefab
        {
            public CharacterType characterType;
            public GameObject gameObject;
        }

        [SerializeField]
        private RunnerPrefab[] m_runnerPrefabs;

        public GameObject Get(CharacterType characterType)
        {
            return m_runnerPrefabs.Where((x) => x.characterType == characterType).First().gameObject;
        }
    }
}