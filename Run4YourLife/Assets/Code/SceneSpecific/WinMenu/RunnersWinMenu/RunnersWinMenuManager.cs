using TMPro;
using UnityEngine;
using System.Collections.Generic;

using Run4YourLife.Player;
using Run4YourLife.GameManagement;
using Run4YourLife.InputManagement;

namespace Run4YourLife.SceneSpecific.WinMenu
{
    [RequireComponent(typeof(RunnerPrefabManager))]
    public class RunnersWinMenuManager : WinMenuManager
    {
        [SerializeField]
        private GameObject runnerSlotPrefab;

        [SerializeField]
        private GameObject[] spawnPoints;

        [SerializeField]
        private float winnerScale = 5.0f;

        [SerializeField]
        private float othersScale = 3.0f;

        [SerializeField]
        private string winnerAnimation = "dance";

        [SerializeField]
        private string othersAnimation = "idle";

        private RunnerPrefabManager runnerPrefabManager;

        void Awake()
        {
            runnerPrefabManager = GetComponent<RunnerPrefabManager>();         
        }

        private void Start()
        {
            if(PlayerManager.Instance.PlayerHandles.Count == 0)
            {
                SetUpFakePlayers(3);
                SetUpFakeScores();
            }
            SpawnRunners();
        }

        private void SetUpFakePlayers(uint numRunners)
        {
            for(uint i = 0; i < numRunners; ++i)
            {
                PlayerManager.Instance.AddPlayer(new PlayerHandle()
                {
                    CharacterType = (CharacterType)i,
                    ID = i + 1,
                    inputDevice = InputDeviceManager.Instance.InputDevices[(int)i],
                    IsBoss = false
                });
            }
        }

        private void SetUpFakeScores()
        {
            ScoreManager.Instance.Initialize();

            foreach(PlayerHandle playerHandle in PlayerManager.Instance.RunnerPlayerHandles)
            {
                ScoreManager.Instance.OnScoreAdded(playerHandle, Random.Range(0.0f, 50.0f));
            }
        }

        private void SpawnRunners()
        {
            List<KeyValuePair<float, PlayerHandle>> points = new List<KeyValuePair<float, PlayerHandle>>();

            foreach(PlayerHandle playerHandle in PlayerManager.Instance.RunnerPlayerHandles)
            {
                points.Add(new KeyValuePair<float, PlayerHandle>(ScoreManager.Instance.GetPointsByplayerHandle(playerHandle), playerHandle));
            }

            points.Sort((a, b) => b.Key.CompareTo(a.Key));

            bool isWinner = true;

            for(int i = 0; i < points.Count; ++i)
            {
                GameObject runnerSlot = Instantiate(runnerSlotPrefab, spawnPoints[i].transform, false);
                GameObject runner = Instantiate(runnerPrefabManager.Get(points[i].Value.CharacterType), runnerSlot.transform, false);

                float scale = othersScale;

                if(isWinner)
                {
                    isWinner = false;
                    scale = winnerScale;
                    runner.GetComponent<Animator>().Play(winnerAnimation);
                }
                else
                {
                    runner.GetComponent<Animator>().Play(othersAnimation);
                }

                runnerSlot.transform.localScale = scale * Vector3.one;

                runnerSlot.GetComponentInChildren<TextMeshPro>().text = ((int)Mathf.Round(points[i].Key)).ToString();
            }
        }
    }
}