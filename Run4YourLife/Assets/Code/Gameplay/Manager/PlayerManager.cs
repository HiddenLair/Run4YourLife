using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace Run4YourLife.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public class PlayerChangedEvent : UnityEvent<PlayerDefinition>
        { }

        private List<PlayerDefinition> players = new List<PlayerDefinition>();

        public PlayerChangedEvent OnPlayerChanged = new PlayerChangedEvent();

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

        public void ClearPlayers()
        {
            players.Clear();
        }

        public void SetPlayerAsBoss(PlayerDefinition player)
        {
            player.IsBoss = true;
            OnPlayerChanged.Invoke(player);
        }

        public void SetPlayerAsRunner(PlayerDefinition player)
        {
            player.IsBoss = false;
            OnPlayerChanged.Invoke(player);
        }

        public void AddPlayer(PlayerDefinition playerDefinition)
        {
            players.Add(playerDefinition);
            OnPlayerChanged.Invoke(playerDefinition);
        }
    }
}