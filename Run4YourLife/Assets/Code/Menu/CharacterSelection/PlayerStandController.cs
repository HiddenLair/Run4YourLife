using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.Input;
using System;

namespace Run4YourLife.CharacterSelection
{
    [RequireComponent(typeof(PlayerStandControllerControlScheme))]
    public class PlayerStandController : MonoBehaviour, IPlayerDefinitionEvents
    {

        #region References

        private CharacterSelectionManager m_characterSelectionManager;
        private PlayerDefinition m_playerDefinition;
        private PlayerManager m_playerManager;
        private PlayerStandControllerControlScheme m_controlScheme;

        #endregion

        #region Stands

        [SerializeField]
        private GameObject redRunner;

        [SerializeField]
        private GameObject greenRunner;

        [SerializeField]
        private GameObject blueRunner;

        [SerializeField]
        private GameObject orangeRunner;

        private GameObject m_activeStand;

        #endregion


        void Awake()
        {
            m_characterSelectionManager = Component.FindObjectOfType<CharacterSelectionManager>();
            Debug.Assert(m_characterSelectionManager != null);

            m_playerManager = Component.FindObjectOfType<PlayerManager>();
            Debug.Assert(m_playerManager != null);

            m_controlScheme = GetComponent<PlayerStandControllerControlScheme>();
        }

        public void OnPlayerDefinitionChanged(PlayerDefinition playerDefinition)
        {
            if(playerDefinition == null)
            {
                ClearPlayerDefinition();
            }
            else
            {
                SetPlayerDefinition(playerDefinition);
            }
        }

        public void SetPlayerDefinition(PlayerDefinition playerDefinition)
        {
            m_playerDefinition = playerDefinition;
            m_activeStand = SpawnPlayerStand(playerDefinition);
            m_controlScheme.Active = true;
        }

        public void ClearPlayerDefinition()
        {
            m_playerDefinition = null;
            Destroy(m_activeStand);
            m_activeStand = null;
            m_controlScheme.Active = false;
        }

        private GameObject SpawnPlayerStand(PlayerDefinition player)
        {
            GameObject prefab = GetStandPrefabForPlayer(player);
            GameObject instance = Instantiate(prefab, transform, false);
            if (player.IsBoss)
            {
                //TODO
                //BossDecoration bossDecoration = instance.GetComponent<BossDecoration>();
                //bossDecoration.enable
            }
            return instance;
        }

        private GameObject GetStandPrefabForPlayer(PlayerDefinition player)
        {
            GameObject prefab = null;
            switch (player.CharacterType)
            {
                case CharacterType.Blue:
                    prefab = blueRunner;
                    break;
                case CharacterType.Red:
                    prefab = redRunner;
                    break;
                case CharacterType.Orange:
                    prefab = orangeRunner;
                    break;
                case CharacterType.Green:
                    prefab = greenRunner;
                    break;
            }

            Debug.Assert(prefab != null);

            return prefab;
        }

        void Update()
        {
            if (m_playerDefinition != null)
            {
                UpdatePlayer();
            }
        }

        private void UpdatePlayer()
        {            
            if (m_controlScheme.getBoss.Started())
            {
                m_playerManager.SetPlayerAsBoss(m_playerDefinition);
            }
            else if (m_controlScheme.leaveGame.Started())
            {
                m_playerManager.RemovePlayer(m_playerDefinition);
            }
            else if (m_controlScheme.nextStand.Started())
            {
                ChangePlayerCharacter(AdvanceType.Next);
            }
            else if(m_controlScheme.previousStand.Started())
            {
                ChangePlayerCharacter(AdvanceType.Previous);
            }
            else if(m_controlScheme.startGame.Started())
            {
                m_characterSelectionManager.OnGameStart();
            }
            else if(m_controlScheme.mainMenu.Started())
            {
                m_characterSelectionManager.OnMainMenuStart();
            }
        }

        private enum AdvanceType
        {
            Next = 1,
            Previous = -1
        }

        private void ChangePlayerCharacter(AdvanceType advanceType)
        {
            PlayerDefinition playerDefinition = this.m_playerDefinition;
            ClearPlayerDefinition();
            playerDefinition.CharacterType = GetDeltaCharacterType(playerDefinition.CharacterType, (int)advanceType);
            SetPlayerDefinition(playerDefinition);
        }

        private CharacterType GetDeltaCharacterType(CharacterType characterType, int delta)
        {
            int nEnumElements = Enum.GetValues(typeof(CharacterType)).Length;
            int newCharacterTypeIndex = (nEnumElements + ((int)characterType) + delta) % nEnumElements;
            return (CharacterType)newCharacterTypeIndex;
        }
    }
}