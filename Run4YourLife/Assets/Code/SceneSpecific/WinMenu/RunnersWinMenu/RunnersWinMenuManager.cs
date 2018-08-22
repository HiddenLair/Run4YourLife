using UnityEngine;
using System.Collections.Generic;

using Run4YourLife.Player;
using Run4YourLife.InputManagement;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.SceneSpecific.WinMenu
{
    [RequireComponent(typeof(RunnerPrefabManager))]
    public class RunnersWinMenuManager : WinMenuManager
    {
        [SerializeField]
        private GameObject[] spawnPoints;

        private RunnerPrefabManager runnerPrefabManager;

        void Awake()
        {
            runnerPrefabManager = GetComponent<RunnerPrefabManager>();

            if (m_sceneMusic != null)
            {
                AudioManager.Instance.PlayMusic(m_sceneMusic);
            }

            if (m_characterSound != null)
            {
                AudioManager.Instance.PlaySFX(m_characterSound);
            }
        }

        private void Start()
        {
            if (PlayerManager.Instance.PlayerHandles.Count == 0)
            {
                SetUpTestPlayers(3);
            }

            SpawnRunners();
        }

        private void SetUpTestPlayers(uint numRunners)
        {
            Debug.Log("Setting up test players");

            for (uint i = 0; i < numRunners; ++i)
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

            for (int i = 0; i < runnerPlayerHandles.Count; ++i)
            {
                GameObject runner = Instantiate(runnerPrefabManager.GetRunner(runnerPlayerHandles[i].CharacterType), spawnPoints[i].transform, false);
                runner.GetComponent<Animator>().Play("Dance");
            }
        }
    }
}