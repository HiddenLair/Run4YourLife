using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace Run4YourLife.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public class PlayerChangedEvent : UnityEvent<PlayerHandle>
        { }

        private List<PlayerHandle> players = new List<PlayerHandle>();

        public PlayerChangedEvent OnPlayerChanged = new PlayerChangedEvent();

        public List<PlayerHandle> GetPlayers()
        {
            return players;
        }

        public PlayerHandle GetBoss()
        {
            PlayerHandle boss = null;
            foreach(PlayerHandle p in players)
            {
                if (p.IsBoss)
                {
                    boss = p;
                    break;
                }
            }
            return boss;
        }

        public List<PlayerHandle> GetRunners()
        {
            List<PlayerHandle> runners = new List<PlayerHandle>();
            foreach (PlayerHandle p in players)
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

        public void SetPlayerAsBoss(PlayerHandle player)
        {
            player.IsBoss = true;
            OnPlayerChanged.Invoke(player);
        }

        public void SetPlayerAsRunner(PlayerHandle player)
        {
            player.IsBoss = false;
            OnPlayerChanged.Invoke(player);
        }

        public void AddPlayer(PlayerHandle playerDefinition)
        {
            players.Add(playerDefinition);
            OnPlayerChanged.Invoke(playerDefinition);
        }
    }
}