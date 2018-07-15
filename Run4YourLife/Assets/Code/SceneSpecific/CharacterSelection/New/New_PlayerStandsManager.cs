using UnityEngine;
using System.Collections.Generic;

using Run4YourLife.Player;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    public class New_PlayerStandsManager : SingletonMonoBehaviour<New_PlayerStandsManager>
    {
        private Dictionary<PlayerHandle, bool> playerSelection = new Dictionary<PlayerHandle, bool>();
        private Dictionary<PlayerHandle, New_CellData> playerCurrentCell = new Dictionary<PlayerHandle, New_CellData>();

        void Start() // OnEnable
        {
            PlayerManager.Instance.OnPlayerChanged.AddListener(OnPlayerChanged);
        }

        void OnDisable()
        {
            PlayerManager.Instance.OnPlayerChanged.RemoveListener(OnPlayerChanged);
        }

        private void OnPlayerChanged(PlayerHandle playerHandle)
        {
            Debug.Log("OnPlayerChanged");
        }

        #region Cell management

        public void OnSelect(PlayerHandle playerHandle)
        {
            // The Player P wants to select the Character C
        }

        public void OnUnselect(PlayerHandle playerHandle)
        {
            // The Player P wants to unselect the Character C
        }

        public void OnReady(PlayerHandle playerHandle)
        {
            // The Player P wants to start the game
        }

        public void OnUp(PlayerHandle playerHandle)
        {
            // Player P wants to preview the Character C (up)

            if(!playerSelection[playerHandle])
            {
                New_CellData cellData = playerCurrentCell[playerHandle];
                playerCurrentCell[playerHandle] = cellData.navigationUp != null ? cellData.navigationUp : cellData;
            }
        }

        public void OnDown(PlayerHandle playerHandle)
        {
            // Player P wants to preview the Character C (down)

            if(!playerSelection[playerHandle])
            {
                New_CellData cellData = playerCurrentCell[playerHandle];
                playerCurrentCell[playerHandle] = cellData.navigationDown != null ? cellData.navigationDown : cellData;
            }
        }

        public void OnLeft(PlayerHandle playerHandle)
        {
            // Player P wants to preview the Character C (left)

            if(!playerSelection[playerHandle])
            {
                New_CellData cellData = playerCurrentCell[playerHandle];
                playerCurrentCell[playerHandle] = cellData.navigationLeft != null ? cellData.navigationLeft : cellData;
            }
        }

        public void OnRight(PlayerHandle playerHandle)
        {
            // Player P wants to preview the Character C (right)

            if(!playerSelection[playerHandle])
            {
                New_CellData cellData = playerCurrentCell[playerHandle];
                playerCurrentCell[playerHandle] = cellData.navigationRight != null ? cellData.navigationRight : cellData;
            }
        }

        #endregion
    }
}