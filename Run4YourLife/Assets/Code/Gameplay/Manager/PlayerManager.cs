using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace Run4YourLife.Player
{
    public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
    {
        public class PlayerChangedEvent : UnityEvent<PlayerHandle> { }

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
            player.CharacterType = GetFirstAviablePlayerCharacterType();
            player.IsBoss = false;
            OnPlayerChanged.Invoke(player);
        }

        public CharacterType GetFirstAviablePlayerCharacterType()
        {
            return GetFirstAviableCharacterType(CharacterType.Purple, 1);
        }

        public CharacterType GetFirstAviableCharacterType(CharacterType characterType, int direction)
        {
            CharacterType ret = characterType;

            int characterTypeI = (int)characterType;
            int characterTypeCount = Enum.GetValues(typeof(CharacterType)).Length;

            for(int i = 0; i < characterTypeCount; ++i)
            {
                int currentCharacterTypeI = characterTypeI + direction * i;

                currentCharacterTypeI %= characterTypeCount;
                if(currentCharacterTypeI < 0)
                {
                    currentCharacterTypeI += characterTypeCount;
                }

                CharacterType currentCharacterType = (CharacterType)currentCharacterTypeI;

                if(!PlayerHasCharacterType(currentCharacterType))
                {
                    ret = currentCharacterType;
                    break;
                }
            }

            return ret;
        }

        private bool PlayerHasCharacterType(CharacterType characterType)
        {
            foreach(PlayerHandle player in GetRunners())
            {
                if(player.CharacterType.Equals(characterType))
                {
                    return true;
                }
            }
            return false;
        }

        public void AddPlayer(PlayerHandle playerDefinition)
        {
            players.Add(playerDefinition);
            OnPlayerChanged.Invoke(playerDefinition);
        }
    }
}