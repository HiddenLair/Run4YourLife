using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

using Run4YourLife.Player;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    public class New_PlayerStandsManager : SingletonMonoBehaviour<New_PlayerStandsManager>
    {
        [SerializeField]
        private New_CellData[] bossCellDatas;

        [SerializeField]
        private New_CellData[] runnerCellDatas;

        [SerializeField]
        private New_PlayerStandController[] playerStandControllers;

        private Dictionary<PlayerHandle, New_CellData> playerCurrentCellDatas = new Dictionary<PlayerHandle, New_CellData>();
        private Dictionary<New_CellData, HashSet<PlayerHandle>> bossCellCurrentContainedPlayers = new Dictionary<New_CellData, HashSet<PlayerHandle>>();
        private Dictionary<New_CellData, HashSet<PlayerHandle>> runnerCellCurrentContainedPlayers = new Dictionary<New_CellData, HashSet<PlayerHandle>>();

        void Awake()
        {
            foreach(New_CellData cellData in bossCellDatas)
            {
                bossCellCurrentContainedPlayers.Add(cellData, new HashSet<PlayerHandle>());
            }

            foreach(New_CellData cellData in runnerCellDatas)
            {
                runnerCellCurrentContainedPlayers.Add(cellData, new HashSet<PlayerHandle>());
            }
        }

        void Start() // OnEnable
        {
            PlayerManager.Instance.OnPlayerChanged.AddListener(OnPlayerAdded);
        }

        void OnDisable()
        {
            PlayerManager.Instance.OnPlayerChanged.RemoveListener(OnPlayerAdded);
        }

        private void OnPlayerAdded(PlayerHandle playerHandle)
        {
            FillStand(playerHandle);

            playerCurrentCellDatas.Add(playerHandle, null);
            UpdateCurrentCell(playerHandle, FindFreeCellForPlayer());
        }

        private void FillStand(PlayerHandle playerHandle)
        {
            foreach(New_PlayerStandController playerStandController in playerStandControllers)
            {
                if(playerStandController.PlayerHandle == null)
                {
                    ExecuteEvents.Execute<IPlayerHandleEvent>(playerStandController.gameObject, null, (a, b) => a.OnPlayerHandleChanged(playerHandle));
                    break;
                }
            }
        }

        private void UpdateCurrentCell(PlayerHandle playerHandle, New_CellData cellData)
        {
            New_CellData previousCellData = playerCurrentCellDatas[playerHandle];

            if(previousCellData != null)
            {
                if(bossCellCurrentContainedPlayers.ContainsKey(previousCellData))
                {
                    bossCellCurrentContainedPlayers[previousCellData].Remove(playerHandle);
                }

                if(runnerCellCurrentContainedPlayers.ContainsKey(previousCellData))
                {
                    runnerCellCurrentContainedPlayers[previousCellData].Remove(playerHandle);
                }
            }

            if(bossCellCurrentContainedPlayers.ContainsKey(cellData))
            {
                bossCellCurrentContainedPlayers[cellData].Add(playerHandle);
            }

            if(runnerCellCurrentContainedPlayers.ContainsKey(cellData))
            {
                runnerCellCurrentContainedPlayers[cellData].Add(playerHandle);
            }

            playerCurrentCellDatas[playerHandle] = cellData;

            foreach(New_PlayerStandController playerStandController in playerStandControllers)
            {
                if(playerStandController.PlayerHandle == playerHandle)
                {
                    playerStandController.SetCharacter(cellData.characterPrefab);
                    break;
                }
            }
        }

        private New_CellData FindFreeCellForPlayer()
        {
            foreach(New_CellData cellData in runnerCellDatas)
            {
                if(runnerCellCurrentContainedPlayers[cellData].Count == 0)
                {
                    return cellData;
                }
            }

            foreach(New_CellData cellData in bossCellDatas)
            {
                if(bossCellCurrentContainedPlayers[cellData].Count == 0)
                {
                    return cellData;
                }
            }

            return runnerCellDatas[0];
        }

        #region Player input management

        public void OnPlayerInputSelect(PlayerHandle playerHandle)
        {
            Debug.Log("OnPlayerInputSelect: " + playerHandle.InputDevice.ID);
        }

        public void OnPlayerInputUnselect(PlayerHandle playerHandle)
        {
            Debug.Log("OnPlayerInputUnselect: " + playerHandle.InputDevice.ID);
        }

        public void OnPlayerInputReady(PlayerHandle playerHandle)
        {
            Debug.Log("OnPlayerInputReady: " + playerHandle.InputDevice.ID);
        }

        public void OnPlayerInputUp(PlayerHandle playerHandle)
        {
            Debug.Log("OnPlayerInputUp: " + playerHandle.InputDevice.ID);
        }

        public void OnPlayerInputDown(PlayerHandle playerHandle)
        {
            Debug.Log("OnPlayerInputDown: " + playerHandle.InputDevice.ID);
        }

        public void OnPlayerInputLeft(PlayerHandle playerHandle)
        {
            Debug.Log("OnPlayerInputLeft: " + playerHandle.InputDevice.ID);
        }

        public void OnPlayerInputRight(PlayerHandle playerHandle)
        {
            Debug.Log("OnPlayerInputRight: " + playerHandle.InputDevice.ID);
        }

        #endregion
    }
}