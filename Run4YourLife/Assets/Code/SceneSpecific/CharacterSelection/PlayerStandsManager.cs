using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

using Run4YourLife.UI;
using Run4YourLife.Utils;
using Run4YourLife.Player;
using Run4YourLife.InputManagement;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    public enum RequestCompletionState
    {
        Error,
        Completed,
        Unmodified
    }

    [RequireComponent(typeof(PlayerPrefabManager))]
    public class PlayerStandsManager : SingletonMonoBehaviour<PlayerStandsManager>
    {
        [SerializeField]
        private CellData[] bossCellDatas;

        [SerializeField]
        private CellData[] runnerCellDatas;

        [SerializeField]
        private PlayerStandController[] playerStandControllers;

        [SerializeField]
        private Image back;

        [SerializeField]
        private OnButtonHeldControlScheme backControlScheme;

        [SerializeField]
        private CanvasGroup[] showOnGameReady;

        [SerializeField]
        private CanvasGroup[] hideOnGameReady;

        [SerializeField]
        private SwapImages bossFace;

        [SerializeField]
        private SwapImages[] runnerFaces;

        [SerializeField]
        private AudioClip audioOnPlayerAdded;

        private PlayerPrefabManager playerPrefabManager;

        private uint currentId = 0;
        private Dictionary<PlayerHandle, uint> playerIds = new Dictionary<PlayerHandle, uint>();

        private Dictionary<PlayerHandle, CellData> playersCurrentCell = new Dictionary<PlayerHandle, CellData>();
        private Dictionary<CellData, HashSet<PlayerHandle>> bossCellCurrentContainedPlayers = new Dictionary<CellData, HashSet<PlayerHandle>>();
        private Dictionary<CellData, HashSet<PlayerHandle>> runnerCellCurrentContainedPlayers = new Dictionary<CellData, HashSet<PlayerHandle>>();

        private int runnerSelectedCount = 0;
        private Dictionary<PlayerHandle, bool> playerSelectionDone = new Dictionary<PlayerHandle, bool>();

        private bool gameStarting = false;
        private bool previousIsGameReady = false;

        void Awake()
        {
            playerPrefabManager = GetComponent<PlayerPrefabManager>();

            foreach(CellData cellData in bossCellDatas)
            {
                bossCellCurrentContainedPlayers.Add(cellData, new HashSet<PlayerHandle>());
            }

            foreach(CellData cellData in runnerCellDatas)
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

        public bool IsOnBossCell(PlayerHandle playerHandle)
        {
            return playersCurrentCell[playerHandle].isBoss;
        }

        public bool IsSelected(PlayerHandle playerHandle)
        {
            return playerSelectionDone[playerHandle];
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

            if(audioOnPlayerAdded != null)
            {
                AudioManager.Instance.PlaySFX(audioOnPlayerAdded);
            }

            FillStand(playerHandle);

            if(currentId == 0)
            {
                bossFace.GetComponent<Image>().enabled = true;
                runnerFaces[0].GetComponent<Image>().enabled = true;
            }
            else if(currentId >= 2)
            {
                runnerFaces[currentId - 1].GetComponent<Image>().enabled = true;
            }

            playerIds.Add(playerHandle, currentId++);

            playersCurrentCell.Add(playerHandle, null);
            playerSelectionDone.Add(playerHandle, false);

            UpdateCurrentCell(playerHandle, FindFreeCellForPlayer());

            UpdateGameReady();
        }

        private void FillStand(PlayerHandle playerHandle)
        {
            foreach(PlayerStandController playerStandController in playerStandControllers)
            {
                if(playerStandController.PlayerHandle == null)
                {
                    ExecuteEvents.Execute<IPlayerHandleEvent>(playerStandController.gameObject, null, (a, b) => a.OnPlayerHandleChanged(playerHandle));
                    break;
                }
            }
        }

        private RequestCompletionState UpdateCurrentCell(PlayerHandle playerHandle, CellData cellData)
        {
            if(cellData != null)
            {
                CellData previousCellData = playersCurrentCell[playerHandle];

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

                    previousCellData.GetComponentInChildren<CellPlayersImageController>().Hide(playerIds[playerHandle]);
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

                cellData.GetComponentInChildren<CellPlayersImageController>().Show(playerIds[playerHandle]);

                foreach(PlayerStandController playerStandController in playerStandControllers)
                {
                    if(playerStandController.PlayerHandle == playerHandle)
                    {
                        return playerStandController.SetCharacter(playerPrefabManager.Get(cellData.characterType, cellData.isBoss));
                    }
                }
            }

            return RequestCompletionState.Unmodified;
        }

        private CellData FindFreeCellForPlayer()
        {
            if(RunnerCanBeSelected())
            {
                foreach(CellData cellData in runnerCellDatas)
                {
                    if(runnerCellCurrentContainedPlayers[cellData].Count == 0)
                    {
                        return cellData;
                    }
                }
            }

            if(BossCanBeSelected())
            {
                foreach(CellData cellData in bossCellDatas)
                {
                    if(bossCellCurrentContainedPlayers[cellData].Count == 0)
                    {
                        return cellData;
                    }
                }
            }

            return runnerCellDatas[0];
        }

        #region Update game ready info

        private void UpdateGameReady()
        {
            UpdatePlayerFacesGameReady();

            bool isGameReady = IsGameReady();

            if(isGameReady != previousIsGameReady)
            {
                if(!isGameReady)
                {
                    ShowOnGameReady(isGameReady);
                    HideOnGameReady(isGameReady);
                }
                else
                {
                    StartCoroutine(YieldHelper.WaitForSeconds(() =>
                    {
                        if(isGameReady = IsGameReady())
                        {
                            ShowOnGameReady(isGameReady);
                            HideOnGameReady(isGameReady);
                        }
                    }, 0.25f));
                }

                previousIsGameReady = isGameReady;
            }
        }

        private void ShowOnGameReady(bool isGameReady)
        {
            foreach(CanvasGroup canvasGroup in showOnGameReady)
            {
                canvasGroup.alpha = Convert.ToSingle(isGameReady);
            }
        }

        private void HideOnGameReady(bool isGameReady)
        {
            foreach(CanvasGroup canvasGroup in hideOnGameReady)
            {
                canvasGroup.alpha = Convert.ToSingle(!isGameReady);
            }
        }

        private void UpdatePlayerFacesGameReady()
        {
            bossFace.ResetInitial();

            foreach(SwapImages runnerFace in runnerFaces)
            {
                runnerFace.ResetInitial();
            }

            uint currentRunnerIndex = 0;

            foreach(KeyValuePair<PlayerHandle, bool> currentPlayerSelectionDone in playerSelectionDone)
            {
                if(currentPlayerSelectionDone.Value)
                {
                    bool isBoss = playersCurrentCell[currentPlayerSelectionDone.Key].isBoss;

                    if(isBoss)
                    {
                        bossFace.Swap();
                    }
                    else
                    {
                        runnerFaces[currentRunnerIndex++].Swap();
                    }
                }
            }
        }

        #endregion

        private bool IsGameReady()
        {
            // If 2+ players
            // 1 Boss selected
            // 1+ Different runners selected

            return currentId >= 2 && AllReady();
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

        private bool BossCanBeSelected()
        {
            foreach(KeyValuePair<PlayerHandle, CellData> currentPlayerCurrentCell in playersCurrentCell)
            {
                if(currentPlayerCurrentCell.Value != null && currentPlayerCurrentCell.Value.isBoss && playerSelectionDone[currentPlayerCurrentCell.Key])
                {
                    return false;
                }
            }

            return true;
        }

        private bool RunnerCanBeSelected()
        {
            int runnerCount = 0;

            foreach(KeyValuePair<PlayerHandle, CellData> currentPlayerCurrentCell in playersCurrentCell)
            {
                if(currentPlayerCurrentCell.Value != null && !currentPlayerCurrentCell.Value.isBoss && playerSelectionDone[currentPlayerCurrentCell.Key])
                {
                    ++runnerCount;
                }
            }

            return runnerCount < Math.Max(currentId, 2) - 1;
        }

        private void PreparePlayers()
        {
            PlayerHandle bossHandle = null;

            foreach(KeyValuePair<PlayerHandle, CellData> currentPlayerCurrentCell in playersCurrentCell)
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

            CellData playerCell = playersCurrentCell[playerHandle];

            // Check if a Boss / Runner can be selected

            if(playerCell.isBoss)
            {
                if(!BossCanBeSelected())
                {
                    return RequestCompletionState.Error;
                }
            }
            else
            {
                if(!RunnerCanBeSelected())
                {
                    return RequestCompletionState.Error;
                }
            }

            // Check if the Boss / Runner has already been selected

            foreach(KeyValuePair<PlayerHandle, CellData> playerCurrentCell in playersCurrentCell)
            {
                if(playerCurrentCell.Value == playerCell && playerSelectionDone[playerCurrentCell.Key])
                {
                    return RequestCompletionState.Error;
                }
            }

            playerSelectionDone[playerHandle] = true;
            playerCell.GetComponentInChildren<SwapImages>().Swap();
            playerCell.GetComponentInChildren<CellPlayersImageController>().Scale(playerIds[playerHandle]);

            UpdateGameReady();

            if(playerCell.isBoss)
            {
                bossFace.GetComponent<ScaleOnTick>().Tick();
            }
            else
            {
                runnerFaces[runnerSelectedCount++].GetComponent<ScaleOnTick>().Tick();
            }

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

            CellData playerCell = playersCurrentCell[playerHandle];

            playerSelectionDone[playerHandle] = false;
            playerCell.GetComponentInChildren<SwapImages>().Swap();

            UpdateGameReady();

            if(!playerCell.isBoss)
            {
                --runnerSelectedCount;
            }

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