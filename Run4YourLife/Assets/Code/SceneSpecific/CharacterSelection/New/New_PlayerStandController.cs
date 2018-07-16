using UnityEngine;

using Run4YourLife.Utils;
using Run4YourLife.Player;
using Run4YourLife.InputManagement;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    [RequireComponent(typeof(New_PlayerStandControllerControlScheme))]
    public class New_PlayerStandController : MonoBehaviour, IPlayerHandleEvent
    {
        [SerializeField]
        private float rotationSpeed;

        [SerializeField]
        private Transform spawnPosition;

        private PlayerHandle playerHandle;
        private New_PlayerStandsManager playerStandsManager;
        private New_PlayerStandControllerControlScheme playerStandControlScheme;

        private GameObject currentCharacter;
        private GameObject currentCharacterPrefab;

        private bool checkVerticalPlayerInput = true;
        private bool checkHorizontalPlayerInput = true;

        private float rotationY = 0.0f;

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

                if(currentCharacter != null)
                {
                    currentCharacter.transform.rotation = Quaternion.AngleAxis(rotationY, Vector3.up);
                }
            }
        }

        public void OnPlayerHandleChanged(PlayerHandle playerHandle)
        {
            this.playerHandle = playerHandle;
            StartCoroutine(YieldHelper.SkipFrame(() => playerStandControlScheme.Active = playerHandle != null));
        }

        public void SetCharacter(GameObject characterPrefab)
        {
            rotationY = 0.0f;

            if(characterPrefab != currentCharacterPrefab)
            {
                if(currentCharacter != null)
                {
                    Destroy(currentCharacter);
                }

                currentCharacter = Instantiate(characterPrefab, spawnPosition, false);

                currentCharacterPrefab = characterPrefab;
            }
        }

        private void PlayerInput()
        {
            #region Select, Unselect and Ready

            if(playerStandControlScheme.Select.Started())
            {
                playerStandsManager.OnPlayerInputSelect(playerHandle);
            }

            if(playerStandControlScheme.Unselect.Started())
            {
                if(playerStandsManager.OnPlayerInputUnselect(playerHandle))
                {
                    rotationY = 0.0f;
                }
            }

            if(playerStandControlScheme.Ready.Started())
            {
                playerStandsManager.OnPlayerInputReady(playerHandle);
            }

            #endregion

            #region Move Up, Down, Left and Right

            float threshold = 0.95f;
            float currentVertical = playerStandControlScheme.VerticalStand.Value();
            float currentHorizontal = playerStandControlScheme.HorizontalStand.Value();

            if(checkVerticalPlayerInput)
            {
                if(currentVertical >= threshold)
                {
                    checkVerticalPlayerInput = false;
                    playerStandsManager.OnPlayerInputUp(playerHandle);
                }
                else if(currentVertical <= -threshold)
                {
                    checkVerticalPlayerInput = false;
                    playerStandsManager.OnPlayerInputDown(playerHandle);
                }
            }

            if(checkHorizontalPlayerInput)
            {
                if(currentHorizontal >= threshold)
                {
                    checkHorizontalPlayerInput = false;
                    playerStandsManager.OnPlayerInputRight(playerHandle);
                }
                else if(currentHorizontal <= -threshold)
                {
                    checkHorizontalPlayerInput = false;
                    playerStandsManager.OnPlayerInputLeft(playerHandle);
                }
            }

            if(!checkVerticalPlayerInput && Mathf.Abs(currentVertical) <= 0.1f)
            {
                checkVerticalPlayerInput = true;
            }

            if(!checkHorizontalPlayerInput && Mathf.Abs(currentHorizontal) <= 0.1f)
            {
                checkHorizontalPlayerInput = true;
            }

            #endregion

            #region Rotate

            if(playerStandsManager.CanRotate(playerHandle))
            {
                rotationY += rotationSpeed * playerStandControlScheme.Rotate.Value() * Time.deltaTime;
            }

            #endregion
        }
    }
}