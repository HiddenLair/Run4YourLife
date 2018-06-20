using UnityEngine;
using System.Collections.Generic;

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

        public List<GameObject> ActivateRunners()
        {            
            int index = 0;
            List<GameObject> runners = new List<GameObject>();
            foreach (PlayerHandle runnerPlayerHandle in PlayerManager.Instance.RunnerPlayerHandles)
            {
                runners.Add(GameplayPlayerManager.Instance.OnRunnerActivate(runnerPlayerHandle, m_runnerSpawns[index].position));
                index++;
            }
            return runners;
        }

        public void ActivateBoss()
        {
            GameplayPlayerManager.Instance.ActivateBoss(m_bossGameObjectPhase, m_bossSpawn.position);
        }
    }
}

