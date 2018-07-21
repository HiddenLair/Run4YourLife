using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

using Run4YourLife.Player;
using Run4YourLife.InputManagement;

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

        [SerializeField]
        private Image back;

        [SerializeField]
        private New_OnButtonHeldControlScheme backControlScheme;

        [SerializeField]
        private CanvasGroup[] showOnGameReady;

        [SerializeField]
        private CanvasGroup[] hideOnGameReady;

        [SerializeField]
        private Image checkOneBoss;

        [SerializeField]
        private Image checkOneOrMoreRunners;

        [SerializeField]
        private Image checkAllReady;

        [SerializeField]
        private Sprite checkReady;

        [SerializeField]
        private Sprite checkNotReady;

        private New_PlayerPrefabManager playerPrefabManager;

        private uint currentId = 0;
        private Dictionary<PlayerHandle, uint> playerIds = new Dictionary<PlayerHandle, uint>();

        private Dictionary<PlayerHandle, New_CellData> playersCurrentCell = new Dictionary<PlayerHandle, New_CellData>();
        private Dictionary<New_CellData, HashSet<PlayerHandle>> bossCellCurrentContainedPlayers = new Dictionary<New_CellData, HashSet<PlayerHandle>>();
        private Dictionary<New_CellData, HashSet<PlayerHandle>> runnerCellCurrentContainedPlayers = new Dictionary<New_CellData, HashSet<PlayerHandle>>();

        private Dictionary<PlayerHandle, bool> playerSelectionDone = new Dictionary<PlayerHandle, bool>();

        private bool gameStarting = false;

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
            UpdateGameReady();
            PlayerManager.Instance.OnPlayerChanged.AddListener(OnPlayerAdded);
        }

        void OnDisable()
        {
            PlayerManager.Instance.OnPlayerChanged.RemoveListener(OnPlayerAdded);
        }

        #region Transitions

        public void OnGoMainMenu()
        {
            if(gameStarting)
            {
                return;
            }

            CharacterSelectionManager.Instance.OnMainMenuStart();
        }

        public void OnGoGame()
        {
            if(gameStarting)
            {
                return;
            }

            if(IsGameReady())
            {
                gameStarting = true;

                PreparePlayers();
                CharacterSelectionManager.Instance.OnGameStart();
            }
        }

        #endregion

        public string GetAnimationNameOnSelected(PlayerHandle playerHandle)
        {
            return playersCurrentCell[playerHandle].animationNameOnSelected;
        }

        public string GetAnimationNameOnNotSelected(PlayerHandle playerHandle)
        {
            return playersCurrentCell[playerHandle].animationNameOnNotSelected;
        }

        public void UpdateBackFillAmount()
        {
            back.fillAmount = backControlScheme.GetPercent();
        }

        private void OnPlayerAdded(PlayerHandle playerHandle)
        {
            if(gameStarting)
            {
                return;
            }

            FillStand(playerHandle);

            playerIds.Add(playerHandle, currentId++);

            playersCurrentCell.Add(playerHandle, null);
            playerSelectionDone.Add(playerHandle, false);

            UpdateCurrentCell(playerHandle, FindFreeCellForPlayer());

            UpdateGameReady();
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

                    previousCellData.GetComponentInChildren<New_CellPlayersImageController>().Hide(playerIds[playerHandle]);
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

                cellData.GetComponentInChildren<New_CellPlayersImageController>().Show(playerIds[playerHandle]);

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

        private void UpdateGameReady()
        {
            checkOneBoss.sprite = OneBoss() ? checkReady : checkNotReady;
            checkOneOrMoreRunners.sprite = OneOrMoreRunners() ? checkReady : checkNotReady;
            checkAllReady.sprite = AllReady() ? checkReady : checkNotReady;

            bool isGameReady = IsGameReady();

            foreach(CanvasGroup canvasGroup in showOnGameReady)
            {
                canvasGroup.alpha = Convert.ToSingle(isGameReady);
            }

            foreach(CanvasGroup canvasGroup in hideOnGameReady)
            {
                canvasGroup.alpha = Convert.ToSingle(!isGameReady);
            }
        }

        private bool IsGameReady()
        {
            return AllReady() && OneBoss() && OneOrMoreRunners();
        }

        private bool OneBoss()
        {
            bool bossFound = false;

            foreach(KeyValuePair<PlayerHandle, New_CellData> currentPlayerCurrentCell in playersCurrentCell)
            {
                bool isBoss = currentPlayerCurrentCell.Value.isBoss;

                if(isBoss)
                {
                    if(bossFound)
                    {
                        return false;
                    }

                    bossFound = true;
                }
            }

            return bossFound;
        }

        private bool OneOrMoreRunners()
        {
            foreach(KeyValuePair<PlayerHandle, New_CellData> currentPlayerCurrentCell in playersCurrentCell)
            {
                if(!currentPlayerCurrentCell.Value.isBoss)
                {
                    return true;
                }
            }

            return false;
        }

        private bool AllReady()
        {
            foreach(KeyValuePair<PlayerHandle, bool> currentPlayerSelectionDone in playerSelectionDone)
            {
                if(!currentPlayerSelectionDone.Value)
                {
                    return false;
                }
            }

            return true;
        }

        private void PreparePlayers()
        {
            PlayerHandle bossHandle = null;

            foreach(KeyValuePair<PlayerHandle, New_CellData> currentPlayerCurrentCell in playersCurrentCell)
            {
                currentPlayerCurrentCell.Key.IsBoss = currentPlayerCurrentCell.Value.isBoss;
                currentPlayerCurrentCell.Key.CharacterType = currentPlayerCurrentCell.Value.characterType;

                if(currentPlayerCurrentCell.Key.IsBoss)
                {
                    bossHandle = currentPlayerCurrentCell.Key;
                }
            }

            PlayerManager.Instance.SetPlayerAsBoss(bossHandle);
        }

        #region Player input management

        public RequestCompletionState OnPlayerInputSelect(PlayerHandle playerHandle)
        {
            if(gameStarting)
            {
                return RequestCompletionState.Error;
            }

            if(playerSelectionDone[playerHandle])
            {
                return RequestCompletionState.Unmodified;
            }

            New_CellData playerCell = playersCurrentCell[playerHandle];

            foreach(KeyValuePair<PlayerHandle, New_CellData> playerCurrentCell in playersCurrentCell)
            {
                if(playerCurrentCell.Value == playerCell && playerSelectionDone[playerCurrentCell.Key])
                {
                    return RequestCompletionState.Error;
                }
            }

            playerSelectionDone[playerHandle] = true;

            UpdateGameReady();

            return RequestCompletionState.Completed;
        }

        public RequestCompletionState OnPlayerInputUnselect(PlayerHandle playerHandle)
        {
            if(gameStarting)
            {
                return RequestCompletionState.Error;
            }

            if(!playerSelectionDone[playerHandle])
            {
                return RequestCompletionState.Unmodified;
            }

            playerSelectionDone[playerHandle] = false;

            UpdateGameReady();

            return RequestCompletionState.Completed;
        }

        public RequestCompletionState OnPlayerInputUp(PlayerHandle playerHandle)
        {
            if(gameStarting)
            {
                return RequestCompletionState.Error;
            }

            if(playerSelectionDone[playerHandle])
            {
                return RequestCompletionState.Error;
            }

            RequestCompletionState requestCompletionState = UpdateCurrentCell(playerHandle, playersCurrentCell[playerHandle].navigationUp);

            if(requestCompletionState == RequestCompletionState.Completed)
            {
                UpdateGameReady();
            }

            return requestCompletionState;
        }

        public RequestCompletionState OnPlayerInputDown(PlayerHandle playerHandle)
        {
            if(gameStarting)
            {
                return RequestCompletionState.Error;
            }

            if(playerSelectionDone[playerHandle])
            {
                return RequestCompletionState.Error;
            }

            RequestCompletionState requestCompletionState = UpdateCurrentCell(playerHandle, playersCurrentCell[playerHandle].navigationDown);

            if(requestCompletionState == RequestCompletionState.Completed)
            {
                UpdateGameReady();
            }

            return requestCompletionState;
        }

        public RequestCompletionState OnPlayerInputLeft(PlayerHandle playerHandle)
        {
            if(gameStarting)
            {
                return RequestCompletionState.Error;
            }

            if(playerSelectionDone[playerHandle])
            {
                return RequestCompletionState.Error;
            }

            RequestCompletionState requestCompletionState = UpdateCurrentCell(playerHandle, playersCurrentCell[playerHandle].navigationLeft);

            if(requestCompletionState == RequestCompletionState.Completed)
            {
                UpdateGameReady();
            }

            return requestCompletionState;
        }

        public RequestCompletionState OnPlayerInputRight(PlayerHandle playerHandle)
        {
            if(gameStarting)
            {
                return RequestCompletionState.Error;
            }

            if(playerSelectionDone[playerHandle])
            {
                return RequestCompletionState.Error;
            }

            RequestCompletionState requestCompletionState = UpdateCurrentCell(playerHandle, playersCurrentCell[playerHandle].navigationRight);

            if(requestCompletionState == RequestCompletionState.Completed)
            {
                UpdateGameReady();
            }

            return requestCompletionState;
        }

        #endregion
    }
}