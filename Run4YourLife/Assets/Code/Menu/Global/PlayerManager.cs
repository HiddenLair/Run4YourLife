using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    public class PlayerManager : MonoBehaviour
    {
        private List<PlayerDefinition> players = new List<PlayerDefinition>();
        private List<PlayerDefinition> playersToDelete = new List<PlayerDefinition>();

        public delegate void dgPlayerChange();
        public event dgPlayerChange OnPlayerChange;

        void LateUpdate()
        {
            if (playersToDelete.Count > 0)
            {
                foreach (PlayerDefinition player in playersToDelete)
                {
                    players.Remove(player);
                }
                playersToDelete.Clear();

                CallOnPlayerChange();
            }
        }

        private void CallOnPlayerChange()
        {
            if (OnPlayerChange != null)
                OnPlayerChange();
        }

        public List<PlayerDefinition> GetPlayers()
        {
            return players;
        }

        public PlayerDefinition GetBoss()
        {
            PlayerDefinition boss = null;
            foreach(PlayerDefinition p in players)
            {
                if (p.IsBoss)
                {
                    boss = p;
                    break;
                }
            }
            return boss;
        }

        public List<PlayerDefinition> GetRunners()
        {
            List<PlayerDefinition> runners = new List<PlayerDefinition>();
            foreach (PlayerDefinition p in players)
            {
                if (!p.IsBoss)
                {
                    runners.Add(p);
                }
            }
            return runners;
        }

        public void SetPlayerAsBoss(PlayerDefinition player)
        {
            foreach (PlayerDefinition p in players)
            {
                p.IsBoss = false;
            }

            player.IsBoss = true;

            CallOnPlayerChange();
        }

        public PlayerDefinition AddPlayer(PlayerDefinition playerDefinition)
        {
            players.Add(playerDefinition);
            CallOnPlayerChange();
            return playerDefinition;
        }

        public void RemovePlayer(PlayerDefinition player)
        {
            playersToDelete.Add(player);
        }
    }
}