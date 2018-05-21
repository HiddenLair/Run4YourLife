using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace Run4YourLife.Player
{
    public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
    {
        public class PlayerChangedEvent : UnityEvent<PlayerHandle> { }

        public PlayerChangedEvent OnPlayerChanged = new PlayerChangedEvent();

        private List<PlayerHandle> m_playerHandles = new List<PlayerHandle>();

        public List<PlayerHandle> PlayerHandles { get { return m_playerHandles; } }
        public int PlayerCount { get { return m_playerHandles.Count; } }

        public List<PlayerHandle> RunnerPlayerHandles {
            get {
                List<PlayerHandle> runners = new List<PlayerHandle>();
                foreach (PlayerHandle p in m_playerHandles)
                {
                    if (!p.IsBoss)
                    {
                        runners.Add(p);
                    }
                }
                return runners;
            }
        }

        public PlayerHandle BossPlayerHandle
        {
            get
            {
                PlayerHandle boss = null;
                foreach (PlayerHandle p in m_playerHandles)
                {
                    if (p.IsBoss)
                    {
                        boss = p;
                        break;
                    }
                }
                return boss;
            }
        }

        public void ClearPlayers()
        {
            m_playerHandles.Clear();
        }

        public void SetPlayerAsBoss(PlayerHandle player)
        {
            player.IsBoss = true;
            UpdatePlayerHandleIds();
            OnPlayerChanged.Invoke(player);
        }

        public void SetPlayerAsRunner(PlayerHandle player)
        {
            player.CharacterType = GetFirstAviablePlayerCharacterType();
            player.IsBoss = false;
            UpdatePlayerHandleIds();
            OnPlayerChanged.Invoke(player);
        }

        private void UpdatePlayerHandleIds()
        {
            uint id = 1;
            foreach(PlayerHandle playerHandle in m_playerHandles)
            {
                if(!playerHandle.IsBoss)
                {
                    playerHandle.ID = id;
                    id++;
                }
            }
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
            foreach(PlayerHandle player in RunnerPlayerHandles)
            {
                if(player.CharacterType.Equals(characterType))
                {
                    return true;
                }
            }
            return false;
        }

        public void AddPlayer(PlayerHandle playerHandle)
        {
            m_playerHandles.Add(playerHandle);
            UpdatePlayerHandleIds();
            OnPlayerChanged.Invoke(playerHandle);
        }
    }
}