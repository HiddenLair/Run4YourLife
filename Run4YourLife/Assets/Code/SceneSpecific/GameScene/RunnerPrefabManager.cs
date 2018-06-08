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
            public GameObject runner;
            public GameObject ghost;
        }

        [SerializeField]
        private RunnerPrefab[] m_runnerPrefabs;

        public GameObject GetRunner(CharacterType characterType)
        {
            return m_runnerPrefabs.Where((x) => x.characterType == characterType).First().runner;
        }
        public GameObject GetGhost(CharacterType characterType)
        {
            return m_runnerPrefabs.Where((x) => x.characterType == characterType).First().ghost;
        }
    }
}