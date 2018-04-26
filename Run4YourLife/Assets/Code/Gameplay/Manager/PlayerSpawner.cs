using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;
using System;

namespace Run4YourLife.GameManagement
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform[] m_runnerSpawns;

        [SerializeField]
        private GamePhase m_bossGameObjectPhase;

        [SerializeField]
        private Transform m_bossSpawn;

        public GameObject[] ActivatePlayers()
        {
            GameObject[] players = new GameObject[PlayerManager.Instance.PlayerCount];
            uint index = 0;

            ActivateRunners(players, ref index);
            players[index] = ActivateBoss();

            return players;
        }

        public GameObject[] ActivateRunners()
        {
            GameObject[] runners = new GameObject[GameplayPlayerManager.Instance.Runners.Count];
            uint index = 0;

            ActivateRunners(runners, ref index);

            return runners;
        }

        private void ActivateRunners(GameObject[] players, ref uint index)
        {
            foreach (PlayerHandle runnerPlayerDefinition in PlayerManager.Instance.RunnerPlayerHandles)
            {
                players[index] = GameplayPlayerManager.Instance.ActivateRunner(runnerPlayerDefinition, m_runnerSpawns[index].position);
                index++;
            }
        }

        public GameObject ActivateBoss()
        {
            return GameplayPlayerManager.Instance.ActivateBoss(m_bossGameObjectPhase, m_bossSpawn.position);
        }
    }
}

