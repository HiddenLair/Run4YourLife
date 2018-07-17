using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

using Run4YourLife.Player;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    public enum RequestCompletionState
    {
        Error,
        Completed,
        Unmodified
    }

    [RequireComponent(typeof(New_PlayerPrefabManager))]
    public class New_PlayerStandsManager : SingletonMonoBehaviour<New_PlayerStandsManager>
    {
        [SerializeField]
        private New_CellData[] bossCellDatas;

        [SerializeField]
        private New_CellData[] runnerCellDatas;

        [SerializeField]
        private New_PlayerStandController[] playerStandControllers;

        private New_PlayerPrefabManager playerPrefabManager;

        private Dictionary<PlayerHandle, New_CellData> playersCurrentCell = new Dictionary<PlayerHandle, New_CellData>();
        private Dictionary<New_CellData, HashSet<PlayerHandle>> bossCellCurrentContainedPlayers = new Dictionary<New_CellData, HashSet<PlayerHandle>>();
        private Dictionary<New_CellData, HashSet<PlayerHandle>> runnerCellCurrentContainedPlayers = new Dictionary<New_CellData, HashSet<PlayerHandle>>();

        private Dictionary<PlayerHandle, bool> playerSelectionDone = new Dictionary<PlayerHandle, bool>();

        void Awake()
        {
            playerPrefabManager = GetComponent<New_PlayerPrefabManager>();

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

            playersCurrentCell.Add(playerHandle, null);
            playerSelectionDone.Add(playerHandle, false);

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

        private RequestCompletionState UpdateCurrentCell(PlayerHandle playerHandle, New_CellData cellData)
        {
            if(cellData != null)
            {
                New_CellData previousCellData = playersCurrentCell[playerHandle];

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

                playersCurrentCell[playerHandle] = cellData;

                foreach(New_PlayerStandController playerStandController in playerStandControllers)
                {
                    if(playerStandController.PlayerHandle == playerHandle)
                    {
                        return playerStandController.SetCharacter(playerPrefabManager.Get(cellData.characterType, cellData.isBoss));
                    }
                }
            }

            return RequestCompletionState.Unmodified;
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

        public RequestCompletionState OnPlayerInputSelect(PlayerHandle playerHandle)
        {
            if(playerSelectionDone[playerHandle])
            {
                Debug.Log("OnPlayerInputSelect: " + playerHandle.InputDevice.ID + " Unmodified");
                return RequestCompletionState.Unmodified;
            }

            New_CellData playerCell = playersCurrentCell[playerHandle];

            foreach(KeyValuePair<PlayerHandle, New_CellData> playerCurrentCell in playersCurrentCell)
            {
                if(playerCurrentCell.Value == playerCell && playerSelectionDone[playerCurrentCell.Key])
                {
                    Debug.Log("OnPlayerInputSelect: " + playerHandle.InputDevice.ID + " Error");
                    return RequestCompletionState.Error;
                }
            }

            playerSelectionDone[playerHandle] = true;

            Debug.Log("OnPlayerInputSelect: " + playerHandle.InputDevice.ID + " Completed");
            return RequestCompletionState.Completed;
        }

        public RequestCompletionState OnPlayerInputUnselect(PlayerHandle playerHandle)
        {
            if(!playerSelectionDone[playerHandle])
            {
                Debug.Log("OnPlayerInputUnselect: " + playerHandle.InputDevice.ID + " Unmodified");
                return RequestCompletionState.Unmodified;
            }

            playerSelectionDone[playerHandle] = false;

            Debug.Log("OnPlayerInputUnselect: " + playerHandle.InputDevice.ID + " Completed");
            return RequestCompletionState.Completed;
        }

        public RequestCompletionState OnPlayerInputUp(PlayerHandle playerHandle)
        {
            if(playerSelectionDone[playerHandle])
            {
                Debug.Log("OnPlayerInputUp: " + playerHandle.InputDevice.ID + " Error");
                return RequestCompletionState.Error;
            }

            RequestCompletionState requestCompletionState = UpdateCurrentCell(playerHandle, playersCurrentCell[playerHandle].navigationUp);

            Debug.Log("OnPlayerInputUp: " + playerHandle.InputDevice.ID + " " + requestCompletionState);
            return requestCompletionState;
        }

        public RequestCompletionState OnPlayerInputDown(PlayerHandle playerHandle)
        {
            if(playerSelectionDone[playerHandle])
            {
                Debug.Log("OnPlayerInputDown: " + playerHandle.InputDevice.ID + " Error");
                return RequestCompletionState.Error;
            }

            RequestCompletionState requestCompletionState = UpdateCurrentCell(playerHandle, playersCurrentCell[playerHandle].navigationDown);

            Debug.Log("OnPlayerInputDown: " + playerHandle.InputDevice.ID + " " + requestCompletionState);
            return requestCompletionState;
        }

        public RequestCompletionState OnPlayerInputLeft(PlayerHandle playerHandle)
        {
            if(playerSelectionDone[playerHandle])
            {
                Debug.Log("OnPlayerInputLeft: " + playerHandle.InputDevice.ID + " Error");
                return RequestCompletionState.Error;
            }

            RequestCompletionState requestCompletionState = UpdateCurrentCell(playerHandle, playersCurrentCell[playerHandle].navigationLeft);

            Debug.Log("OnPlayerInputLeft: " + playerHandle.InputDevice.ID + " " + requestCompletionState);
            return requestCompletionState;
        }

        public RequestCompletionState OnPlayerInputRight(PlayerHandle playerHandle)
        {
            if(playerSelectionDone[playerHandle])
            {
                Debug.Log("OnPlayerInputRight: " + playerHandle.InputDevice.ID + " Error");
                return RequestCompletionState.Error;
            }

            RequestCompletionState requestCompletionState = UpdateCurrentCell(playerHandle, playersCurrentCell[playerHandle].navigationRight);

            Debug.Log("OnPlayerInputRight: " + playerHandle.InputDevice.ID + " " + requestCompletionState);
            return requestCompletionState;
        }

        #endregion

        public bool CanRotate(PlayerHandle playerHandle)
        {
            return playerSelectionDone[playerHandle];
        }

        public string GetAnimationNameOnSelected(PlayerHandle playerHandle)
        {
            return playersCurrentCell[playerHandle].animationNameOnSelected;
        }

        public string GetAnimationNameOnNotSelected(PlayerHandle playerHandle)
        {
            return playersCurrentCell[playerHandle].animationNameOnNotSelected;
        }
    }
}