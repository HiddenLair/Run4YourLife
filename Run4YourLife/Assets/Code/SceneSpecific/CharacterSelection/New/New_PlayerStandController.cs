using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.InputManagement;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    [RequireComponent(typeof(New_PlayerStandControllerControlScheme))]
    public class New_PlayerStandController : MonoBehaviour, IPlayerHandleEvent
    {
        [SerializeField]
        private Transform spawnPosition;

        private PlayerHandle playerHandle;
        private New_PlayerStandsManager playerStandsManager;
        private New_PlayerStandControllerControlScheme playerStandControlScheme;

        private GameObject currentCharacter;

        public PlayerHandle PlayerHandle { get { return playerHandle; } }

        void Awake()
        {
            playerStandsManager = New_PlayerStandsManager.Instance;
            playerStandControlScheme = GetComponent<New_PlayerStandControllerControlScheme>();
        }

        void Update()
        {
            if(playerHandle != null)
            {
                PlayerInput();
            }
        }

        public void OnPlayerHandleChanged(PlayerHandle playerHandle)
        {
            this.playerHandle = playerHandle;
            playerStandControlScheme.Active = playerHandle != null;
        }

        public void SetCharacter(GameObject characterPrefab)
        {
            if(currentCharacter != null)
            {
                Destroy(currentCharacter);
            }

            currentCharacter = Instantiate(characterPrefab, spawnPosition, false);
        }

        private void PlayerInput()
        {
            if(playerStandControlScheme.Select.Started())
            {
                playerStandsManager.OnPlayerInputSelect(playerHandle);
            }

            if(playerStandControlScheme.Unselect.Started())
            {
                playerStandsManager.OnPlayerInputUnselect(playerHandle);
            }

            if(playerStandControlScheme.Ready.Started())
            {
                playerStandsManager.OnPlayerInputReady(playerHandle);
            }
        }
    }
}