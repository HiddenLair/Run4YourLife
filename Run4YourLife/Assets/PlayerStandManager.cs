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

        private List<GameObject> stands = new List<GameObject>();

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
            Debug.Assert(stands.Count == 0);
            foreach (PlayerDefinition player in playerManager.GetPlayers())
            {
                GameObject stand = SpawnPlayerStand(player,standPositions[stands.Count]);
                stands.Add(stand);
            }
        }

        private GameObject SpawnPlayerStand(PlayerDefinition player, Transform transform)
        {
            GameObject prefab = null;
            switch(player.CharacterType)
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
            return instance;
        }

        private void DestroyCurrentStants()
        {
            foreach (GameObject stand in stands)
            {
                Destroy(stand);
            }
            stands.Clear();
        }
    }
}