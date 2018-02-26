using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.Input;
using System;

namespace Run4YourLife.CharacterSelection
{
    public class PlayerStandController : MonoBehaviour
    {
        private PlayerDefinition playerDefinition;
        private PlayerManager playerManager;

        [SerializeField]
        private GameObject redRunner;

        [SerializeField]
        private GameObject greenRunner;

        [SerializeField]
        private GameObject blueRunner;

        [SerializeField]
        private GameObject orangeRunner;

        private GameObject activeStand;

        private PlayerStandControllerControlScheme controlScheme;

        void Awake()
        {
            playerManager = Component.FindObjectOfType<PlayerManager>();
            Debug.Assert(playerManager != null);
        }

        private void Start()
        {

        }

        public void SetPlayerDefinition(PlayerDefinition playerDefinition)
        {
            this.playerDefinition = playerDefinition;
            activeStand = SpawnPlayerStand(playerDefinition);
        }

        public void ClearPlayerDefinition()
        {
            this.playerDefinition = null;
            Destroy(activeStand);
            activeStand = null;
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
            if (playerDefinition != null)
            {
                UpdatePlayer();
            }
        }

        private void UpdatePlayer()
        {            
            if (controlScheme.getBoss.Started())
            {
                playerManager.SetPlayerAsBoss(playerDefinition);
            }
            else if (controlScheme.leaveGame.Started())
            {
                playerManager.RemovePlayer(playerDefinition);
            }
            else if (controlScheme.nextStand.Started())
            {
                ChangePlayerCharacter(AdvanceType.Next);
            }
            else if(controlScheme.previousStand.Started())
            {
                ChangePlayerCharacter(AdvanceType.Previous);
            }
        }

        private enum AdvanceType
        {
            Next,
            Previous
        }

        private void ChangePlayerCharacter(AdvanceType advanceType)
        {
            PlayerDefinition playerDefinition = this.playerDefinition;
            ClearPlayerDefinition();

            int delta = advanceType.Equals(AdvanceType.Next) ? 1 : -1;
            playerDefinition.CharacterType = GetDeltaCharacterType(playerDefinition.CharacterType, delta);
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