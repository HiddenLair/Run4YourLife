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

        private GameplayPlayerManager m_gameplayPlayerManager;

        private void Awake()
        {
            m_gameplayPlayerManager = FindObjectOfType<GameplayPlayerManager>();
            Debug.Assert(m_gameplayPlayerManager != null);
        }

        public GameObject[] ActivatePlayers()
        {
            GameObject[] players = new GameObject[m_gameplayPlayerManager.PlayerCount];
            uint index = 0;

            foreach (PlayerDefinition runnerPlayerDefinition in m_gameplayPlayerManager.RunnerPlayerDefinitions)
            {
                players[index] = m_gameplayPlayerManager.ActivateRunner(runnerPlayerDefinition, m_runnerSpawns[index].position);
                index++;
            }

            players[index] = ActivateBoss();

            return players;
        }

        public GameObject ActivateBoss()
        {
            return m_gameplayPlayerManager.ActivateBoss(m_bossGameObjectPhase, m_bossSpawn.position);
        }
    }
}

