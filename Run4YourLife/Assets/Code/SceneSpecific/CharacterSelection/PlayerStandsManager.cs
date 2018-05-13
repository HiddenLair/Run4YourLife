using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    public class PlayerStandsManager : SingletonMonoBehaviour<PlayerStandsManager>
    {
        [SerializeField]
        private BossStandController bossStandController;

        [SerializeField]
        private RunnerStandController[] runnerStandControllers;

        private bool ready = true;
        private bool gameHasStarted = false;

        private void OnEnable()
        {
            PlayerManager.Instance.OnPlayerChanged.AddListener(OnPlayerChanged);
        }

        private void OnDisable()
        {
            PlayerManager.Instance.OnPlayerChanged.RemoveListener(OnPlayerChanged);
        }

        void Update()
        {
            if(CheckGoGame())
            {
                StartGame();
            }
        }

        void LateUpdate()
        {
            ready = true;
        }

        void OnPlayerChanged(PlayerHandle playerHandle)
        {
            DestroyStand(playerHandle);
            CreateStand(playerHandle);
        }

        void OnPlayerLeft(PlayerHandle playerHandle)
        {
            DestroyStand(playerHandle);
        }

        private void DestroyStand(PlayerHandle playerHandle)
        {
            Debug.Assert(playerHandle != null);

            PlayerStandController playerStandController = null;

            if(bossStandController.PlayerHandle == playerHandle)
            {
                playerStandController = bossStandController;
            }
            else
            {
                foreach(RunnerStandController runnerStandController in runnerStandControllers)
                {
                    if(runnerStandController.PlayerHandle == playerHandle)
                    {
                        playerStandController = runnerStandController;
                        break;
                    }
                }
            }

            if(playerStandController != null)
            {
                ExecuteEvents.Execute<IPlayerHandleEvent>(playerStandController.gameObject, null, (a, b) => a.OnPlayerHandleChanged(null));
            }
        }

        private void CreateStand(PlayerHandle playerHandle)
        {
            PlayerStandController playerStandController = null;

            if(playerHandle.IsBoss)
            {
                Debug.Assert(bossStandController.PlayerHandle == null);

                playerStandController = bossStandController;
            }
            else
            {
                foreach(RunnerStandController runnerStandController in runnerStandControllers)
                {
                    if(runnerStandController.PlayerHandle == null)
                    {
                        playerStandController = runnerStandController;
                        break;
                    }
                }
            }

            Debug.Assert(playerStandController != null);

            ExecuteEvents.Execute<IPlayerHandleEvent>(playerStandController.gameObject, null, (a, b) => a.OnPlayerHandleChanged(playerHandle));
        }

        #region Boss And Runner StandController Management

        public void SetAsBoss(RunnerStandController runnerStandController)
        {
            if(!ready) return;
            ready = false;

            if(bossStandController.PlayerHandle == null)
            {
                PlayerManager.Instance.SetPlayerAsBoss(runnerStandController.PlayerHandle);
            }
        }

        public void SetAsRunner(BossStandController bossStandController)
        {
            if(!ready) return;
            ready = false;

            PlayerManager.Instance.SetPlayerAsRunner(bossStandController.PlayerHandle);
        }

        public void GoMainMenu()
        {
            CharacterSelectionManager.Instance.OnMainMenuStart();
        }

        private bool CheckGoGame()
        {
            if(gameHasStarted)
            {
                return false;
            }

            if(bossStandController.PlayerHandle == null)
            {
                return false;
            }

            if(!bossStandController.IsReady)
            {
                return false;
            }

            bool existsReadyRunner = false;

            foreach(RunnerStandController runnerStandController in runnerStandControllers)
            {
                if(runnerStandController.PlayerHandle != null)
                {
                    if(!runnerStandController.IsReady)
                    {
                        return false;
                    }
                    else
                    {
                        existsReadyRunner = true;
                    }
                }
            }

            if(!existsReadyRunner)
            {
                return false;
            }

            return true;
        }

        private void StartGame()
        {
            gameHasStarted = true;
            CharacterSelectionManager.Instance.OnGameStart();
        }

        #endregion
    }
}