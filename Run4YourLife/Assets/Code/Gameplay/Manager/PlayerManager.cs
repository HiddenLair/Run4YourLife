using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Run4YourLife.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public class PlayerChangedEvent : UnityEvent<PlayerDefinition>
        { }

        public class PlayerLeftEvent : UnityEvent<PlayerDefinition>
        { }

        private List<PlayerDefinition> players = new List<PlayerDefinition>();
        private List<PlayerDefinition> playersToDelete = new List<PlayerDefinition>();

        public PlayerChangedEvent OnPlayerChanged = new PlayerChangedEvent();
        public PlayerLeftEvent OnPlayerLeft = new PlayerLeftEvent();

        void LateUpdate()
        {
            if (playersToDelete.Count > 0)
            {
                foreach (PlayerDefinition player in playersToDelete)
                {
                    players.Remove(player);
                    OnPlayerLeft.Invoke(player);
                }
                playersToDelete.Clear();
            }
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

        public void RemovePlayer(PlayerDefinition player)
        {
            playersToDelete.Add(player);
        }
    }
}