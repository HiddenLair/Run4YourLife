using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;

namespace Run4YourLife.CharacterSelection
{
    public class PlayerStandsManager : SingletonMonoBehaviour<PlayerStandsManager>
    {
        [SerializeField]
        private BossStandController bossStandController;

        [SerializeField]
        private RunnerStandController[] runnerStandControllers;

        private bool ready = true;
        private bool gameHasStarted = false;

        private void Start()
        {
            PlayerManager.Instance.OnPlayerChanged.AddListener(OnPlayerChanged);
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

        void OnPlayerChanged(PlayerHandle playerDefinition)
        {
            DestroyStand(playerDefinition);
            CreateStand(playerDefinition);
        }

        void OnPlayerLeft(PlayerHandle playerDefinition)
        {
            DestroyStand(playerDefinition);
        }

        private void DestroyStand(PlayerHandle playerDefinition)
        {
            PlayerStandController playerStandController = null;

            if(bossStandController.GetPlayerDefinition() == playerDefinition)
            {
                playerStandController = bossStandController;
            }
            else
            {
                foreach(RunnerStandController runnerStandController in runnerStandControllers)
                {
                    if(runnerStandController.GetPlayerDefinition() == playerDefinition)
                    {
                        playerStandController = runnerStandController;
                        break;
                    }
                }
            }

            if(playerStandController != null)
            {
                ExecuteEvents.Execute<IPlayerHandleEvent>(playerStandController.gameObject, null, (a, b) => a.OnPlayerDefinitionChanged(null));
            }
        }

        private void CreateStand(PlayerHandle playerDefinition)
        {
            PlayerStandController playerStandController = null;

            if(playerDefinition.IsBoss)
            {
                Debug.Assert(bossStandController.GetPlayerDefinition() == null);

                playerStandController = bossStandController;
            }
            else
            {
                foreach(RunnerStandController runnerStandController in runnerStandControllers)
                {
                    if(runnerStandController.GetPlayerDefinition() == null)
                    {
                        playerStandController = runnerStandController;
                        break;
                    }
                }
            }

            Debug.Assert(playerStandController != null);

            ExecuteEvents.Execute<IPlayerHandleEvent>(playerStandController.gameObject, null, (a, b) => a.OnPlayerDefinitionChanged(playerDefinition));
        }

        #region Boss And Runner StandController Management

        public void SetAsBoss(RunnerStandController runnerStandController)
        {
            if(!ready) return;
            ready = false;

            if(bossStandController.GetPlayerDefinition() == null)
            {
                PlayerManager.Instance.SetPlayerAsBoss(runnerStandController.GetPlayerDefinition());
            }
        }

        public void SetAsRunner(BossStandController bossStandController)
        {
            if(!ready) return;
            ready = false;

            PlayerManager.Instance.SetPlayerAsRunner(bossStandController.GetPlayerDefinition());
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

            if(bossStandController.GetPlayerDefinition() == null)
            {
                return false;
            }

            if(!bossStandController.GetReady())
            {
                return false;
            }

            bool existsReadyRunner = false;

            foreach(RunnerStandController runnerStandController in runnerStandControllers)
            {
                if(runnerStandController.GetPlayerDefinition() != null)
                {
                    if(!runnerStandController.GetReady())
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