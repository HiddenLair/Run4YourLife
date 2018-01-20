using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public delegate void dgPlayerChange();
        public event dgPlayerChange OnPlayerChange;

        private void CallOnPlayerChange()
        {
            if (OnPlayerChange != null)
                OnPlayerChange();
        }

        private List<PlayerDefinition> players = new List<PlayerDefinition>();

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

        public void AddPlayer(PlayerDefinition player)
        {
            players.Add(player);
            CallOnPlayerChange();
        }

        public void RemovePlayer(PlayerDefinition player)
        {
            players.Remove(player);
            CallOnPlayerChange();
        }
    }
}