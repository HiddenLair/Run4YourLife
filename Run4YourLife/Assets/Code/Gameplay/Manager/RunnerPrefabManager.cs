using System;
using UnityEngine;
using System.Collections.Generic;

namespace Run4YourLife.Player
{
    public enum RunnerPrefabType
    {
        Game,
        CharacterSelection,
        RunnersWin
    }

    public class RunnerPrefabManager : MonoBehaviour
    {
        [Serializable]
        public class RunnerPrefab
        {
            public CharacterType characterType;
            public GameObject gameObject;
        }

        [SerializeField]
        private RunnerPrefab[] gamePrefabs;

        [SerializeField]
        private RunnerPrefab[] characterSelectionPrefabs;

        [SerializeField]
        private RunnerPrefab[] runnersWinPrefabs;

        private Dictionary<KeyValuePair<RunnerPrefabType, CharacterType>, GameObject> runnerPrefabs = new Dictionary<KeyValuePair<RunnerPrefabType, CharacterType>, GameObject>();

        void Awake()
        {
            Build();
        }

        public GameObject Get(RunnerPrefabType runnerPrefabType, CharacterType characterType)
        {
            return runnerPrefabs[new KeyValuePair<RunnerPrefabType, CharacterType>(runnerPrefabType, characterType)];
        }

        private void Build()
        {
            Build(RunnerPrefabType.Game, gamePrefabs);
            Build(RunnerPrefabType.CharacterSelection, characterSelectionPrefabs);
            Build(RunnerPrefabType.RunnersWin, runnersWinPrefabs);
        }

        private void Build(RunnerPrefabType runnerPrefabType, RunnerPrefab[] prefabs)
        {
            foreach(RunnerPrefab runnerPrefab in prefabs)
            {
                runnerPrefabs[new KeyValuePair<RunnerPrefabType, CharacterType>(runnerPrefabType, runnerPrefab.characterType)] = runnerPrefab.gameObject;
            }
        }
    }
}