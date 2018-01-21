using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using System;

namespace Run4YourLife.CharacterSelection
{
    public class PlayerStandManager : MonoBehaviour
    {
        PlayerManager playerManager;

        [SerializeField]
        private Transform[] standPositions;

        [SerializeField]
        private GameObject redRunner;

        [SerializeField]
        private GameObject greenRunner;

        [SerializeField]
        private GameObject blueRunner;

        [SerializeField]
        private GameObject orangeRunner;



        private void Awake()
        {
            playerManager = Component.FindObjectOfType<PlayerManager>();
            playerManager.OnPlayerChange += OnPlayerChange;
        }

        void OnPlayerChange()
        {
            DestroyCurrentStants();
            SpawnNewStands();
        }

        private void SpawnNewStands()
        {
            int standIndex = 0;
            foreach (PlayerDefinition player in playerManager.GetPlayers())
            {
                SpawnPlayerStand(player,standPositions[standIndex]);
                standIndex++;
            }
        }

        private void SpawnPlayerStand(PlayerDefinition player, Transform transform)
        {
            GameObject prefab = null;
            switch(player.characterType)
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
                default:
                    Debug.LogWarning("Character type not found");
                    break;
            }

            Debug.Assert(prefab != null);

            GameObject instance = Instantiate(prefab,transform,false);
            if(player.IsBoss)
            {
                //TODO
                //BossDecoration bossDecoration = instance.GetComponent<BossDecoration>();
                //bossDecoration.enable
            }
        }

        private void DestroyCurrentStants()
        {
            foreach (Transform stand in standPositions)
            {
                if (stand.childCount > 0)
                {
                    Destroy(stand.GetChild(0));
                }
            }
        }
    }
}