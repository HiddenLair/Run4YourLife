using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;

namespace Run4YourLife.CharacterSelection
{
    public class PlayerStandsManager : MonoBehaviour
    {
        [SerializeField]
        private BossStandController bossStandController;

        [SerializeField]
        private RunnerStandController[] runnerStandControllers;

        private PlayerManager playerManager;
        private CharacterSelectionManager characterSelectionManager;

        private bool ready = true;
        private bool gameHasStarted = false;

        void Awake()
        {
            playerManager = FindObjectOfType<PlayerManager>();
            Debug.Assert(playerManager != null);

            characterSelectionManager = FindObjectOfType<CharacterSelectionManager>();
            Debug.Assert(characterSelectionManager != null);

            playerManager.OnPlayerChanged.AddListener(OnPlayerChanged);
        }

        void Update()
        {
            if(CheckGoGame())
            {
                GoGame();
            }
        }

        void LateUpdate()
        {
            ready = true;
        }

        void OnPlayerChanged(PlayerDefinition playerDefinition)
        {
            DestroyStand(playerDefinition);
            CreateStand(playerDefinition);
        }

        void OnPlayerLeft(PlayerDefinition playerDefinition)
        {
            DestroyStand(playerDefinition);
        }

        private void DestroyStand(PlayerDefinition playerDefinition)
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
                ExecuteEvents.Execute<IPlayerDefinitionEvents>(playerStandController.gameObject, null, (a, b) => a.OnPlayerDefinitionChanged(null));
            }
        }

        private void CreateStand(PlayerDefinition playerDefinition)
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

            ExecuteEvents.Execute<IPlayerDefinitionEvents>(playerStandController.gameObject, null, (a, b) => a.OnPlayerDefinitionChanged(playerDefinition));
        }

        #region Boss And Runner StandController Management

        public void SetAsBoss(RunnerStandController runnerStandController)
        {
            if(!ready) return;
            ready = false;

            if(bossStandController.GetPlayerDefinition() == null)
            {
                playerManager.SetPlayerAsBoss(runnerStandController.GetPlayerDefinition());
            }
        }

        public void SetAsRunner(BossStandController bossStandController)
        {
            if(!ready) return;
            ready = false;

            playerManager.SetPlayerAsRunner(bossStandController.GetPlayerDefinition());
        }

        public void GoMainMenu()
        {
            characterSelectionManager.OnMainMenuStart();
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

        private void GoGame()
        {
            gameHasStarted = true;
            characterSelectionManager.OnGameStart();
        }

        #endregion
    }
}