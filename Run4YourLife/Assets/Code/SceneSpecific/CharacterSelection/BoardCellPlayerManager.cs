using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    public class BoardCellPlayerManager : SingletonMonoBehaviour<BoardCellPlayerManager>
    {
        [SerializeField]
        private Image[] playerCellImages;

        [SerializeField]
        private float timeBetweenTicks = 0.5f;

        private List<bool> playersCellUnlocked = new List<bool>();

        void Awake()
        {
            foreach(Image image in playerCellImages)
            {
                playersCellUnlocked.Add(true);
            }
        }

        void Start()
        {
            StartCoroutine(PlayerCellAnimations());
        }

        public void SetPlayerCell(uint playerId, Transform cellTransform)
        {
            playerCellImages[playerId].enabled = true;
            playerCellImages[playerId].transform.SetParent(cellTransform, false);
        }

        public void LockPlayerCell(uint playerId)
        {
            playersCellUnlocked[(int)playerId] = false;
        }

        public void UnlockPlayerCell(uint playerId)
        {
            playersCellUnlocked[(int)playerId] = true;
        }

        private IEnumerator PlayerCellAnimations()
        {
            yield return new WaitForSeconds(timeBetweenTicks);

            for(int i = 0; i < playerCellImages.Length; ++i)
            {
                if(playerCellImages[i].enabled && playersCellUnlocked[i])
                {
                    playerCellImages[i].GetComponent<ScaleOnTick>().Tick();
                }
            }

            StartCoroutine(PlayerCellAnimations());
        }
    }
}