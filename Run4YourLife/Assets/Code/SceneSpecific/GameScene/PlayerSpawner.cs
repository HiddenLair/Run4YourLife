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

        public void ActivatePlayers()
        {
            ActivateRunners();
            ActivateBoss();
        }

        public void ActivateRunners()
        {            
            int index = 0;
            foreach (PlayerHandle runnerPlayerHandle in PlayerManager.Instance.RunnerPlayerHandles)
            {
                GameplayPlayerManager.Instance.OnRunnerActivate(runnerPlayerHandle, m_runnerSpawns[index].position);
                index++;
            }
        }

        public void ActivateBoss()
        {
            GameplayPlayerManager.Instance.ActivateBoss(m_bossGameObjectPhase, m_bossSpawn.position);
        }
    }
}

