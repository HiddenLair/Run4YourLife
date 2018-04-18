﻿using TMPro;
using UnityEngine;
using System.Collections.Generic;

using Run4YourLife.Player;
using Run4YourLife.GameManagement;

namespace Run4YourLife.WinMenu
{
    public class RunnersWinMenuManager : WinMenuManager
    {
        [SerializeField]
        private GameObject runnerSlotPrefab;

        [SerializeField]
        private GameObject runnerPrefab;

        [SerializeField]
        private GameObject[] spawnPoints;

        [SerializeField]
        private float winnerScale = 5.0f;

        [SerializeField]
        private float othersScale = 3.0f;

        [SerializeField]
        private string winnerAnimation = "correr";

        [SerializeField]
        private string othersAnimation = "idle";

        void Awake()
        {
            // SetUpFakePlayers(3); // TEST
            // SetUpFakeScores(); // TEST
            SpawnRunners();
        }

        private void SetUpFakePlayers(uint numRunners)
        {
            PlayerManager playerManager = FindObjectOfType<PlayerManager>();

            if(playerManager == null)
            {
                playerManager = gameObject.AddComponent<PlayerManager>();
            }

            for(uint i = 0; i < numRunners; ++i)
            {
                playerManager.AddPlayer(new PlayerDefinition()
                {
                    CharacterType = CharacterType.Purple,
                    ID = i + 1,
                    inputDevice = new Input.InputDevice(i + 1),
                    IsBoss = false
                });
            }
        }

        private void SetUpFakeScores()
        {
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();

            if(scoreManager == null)
            {
                scoreManager = gameObject.AddComponent<ScoreManager>();
            }

            scoreManager.Initialize();

            foreach(PlayerDefinition playerDefinition in FindObjectOfType<PlayerManager>().GetRunners())
            {
                scoreManager.OnAddPoints(playerDefinition, Random.Range(0.0f, 50.0f));
            }
        }

        private void SpawnRunners()
        {
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            PlayerManager playerManager = FindObjectOfType<PlayerManager>();

            List<KeyValuePair<float, PlayerDefinition>> points = new List<KeyValuePair<float, PlayerDefinition>>();

            foreach(PlayerDefinition playerDefinition in playerManager.GetRunners())
            {
                points.Add(new KeyValuePair<float, PlayerDefinition>(scoreManager.GetPointsByPlayerDefinition(playerDefinition), playerDefinition));
            }

            points.Sort((a, b) => b.Key.CompareTo(a.Key));

            bool isWinner = true;

            for(int i = 0; i < points.Count; ++i)
            {
                GameObject runnerSlot = Instantiate(runnerSlotPrefab, spawnPoints[i].transform, false);

                // Use sorted PlayerDefinition to instantiate the proper runner

                GameObject runner = Instantiate(runnerPrefab, runnerSlot.transform, false);

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