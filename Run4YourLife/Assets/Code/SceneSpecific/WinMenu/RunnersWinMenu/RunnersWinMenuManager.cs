using UnityEngine;
using System.Collections.Generic;

using Run4YourLife.Player;
using Run4YourLife.InputManagement;

namespace Run4YourLife.SceneSpecific.WinMenu
{
    [RequireComponent(typeof(RunnerPrefabManager))]
    public class RunnersWinMenuManager : WinMenuManager
    {
        [SerializeField]
        private GameObject[] spawnPoints;

        [SerializeField]
        private float winnerScale = 5.0f;

        [SerializeField]
        private float othersScale = 3.0f;

        [SerializeField]
        private string winnerAnimation = "dance";

        private RunnerPrefabManager runnerPrefabManager;

        void Awake()
        {
            runnerPrefabManager = GetComponent<RunnerPrefabManager>();         
        }

        private void Start()
        {
            if(PlayerManager.Instance.PlayerHandles.Count == 0)
            {
                SetUpTestPlayers(3);
            }

            SpawnRunners();
        }

        private void SetUpTestPlayers(uint numRunners)
        {
            Debug.Log("Setting up test players");

            for(uint i = 0; i < numRunners; ++i)
            {
                PlayerManager.Instance.AddPlayer(new PlayerHandle()
                {
                    CharacterType = (CharacterType)i,
                    InputDevice = InputDeviceManager.Instance.InputDevices[(int)i],
                    IsBoss = false
                });
            }
        }

        private void SpawnRunners()
        {
            List<PlayerHandle> runnerPlayerHandles = PlayerManager.Instance.RunnerPlayerHandles;

            for(int i = 0; i < runnerPlayerHandles.Count; ++i)
            {
                GameObject runner = Instantiate(runnerPrefabManager.GetRunner(runnerPlayerHandles[i].CharacterType), spawnPoints[i].transform, false);

                float scale = i == 0 ? winnerScale : othersScale;
                runner.transform.localScale = scale * Vector3.one;

                runner.GetComponent<Animator>().Play(winnerAnimation);
            }
        }
    }
}