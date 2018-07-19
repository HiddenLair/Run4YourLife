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

        [SerializeField]
        private GameObject infoIndex;

        [SerializeField]
        private GameObject infoJoin;

        [SerializeField]
        private GameObject infoSelect;

        [SerializeField]
        private GameObject infoUnselect;

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

            InfoShowJoin();
            infoIndex.SetActive(false);
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
            infoIndex.SetActive(true);
            this.playerHandle = playerHandle;
            StartCoroutine(YieldHelper.SkipFrame(() => playerStandControlScheme.Active = playerHandle != null));
        }

        public RequestCompletionState SetCharacter(GameObject characterPrefab)
        {
            rotationY = 0.0f;
            InfoShowSelect();

            if(characterPrefab != currentCharacterPrefab)
            {
                if(currentCharacter != null)
                {
                    Destroy(currentCharacter);
                }

                currentCharacter = Instantiate(characterPrefab, spawnPosition, false);
                currentCharacter.GetComponent<Animator>().Play(playerStandsManager.GetAnimationNameOnNotSelected(playerHandle));

                currentCharacterPrefab = characterPrefab;

                return RequestCompletionState.Completed;
            }

            return RequestCompletionState.Unmodified;
        }

        private void PlayerInput()
        {
            #region Select and Unselect

            if(playerStandControlScheme.Select.Started())
            {
                RequestCompletionState requestCompletionState = playerStandsManager.OnPlayerInputSelect(playerHandle);

                if(requestCompletionState == RequestCompletionState.Completed)
                {
                    rotationY = 0.0f;
                    InfoShowUnselect();
                    currentCharacter.GetComponent<Animator>().Play(playerStandsManager.GetAnimationNameOnSelected(playerHandle));
                }
            }

            if(playerStandControlScheme.Unselect.Started())
            {
                RequestCompletionState requestCompletionState = playerStandsManager.OnPlayerInputUnselect(playerHandle);

                if(requestCompletionState == RequestCompletionState.Completed)
                {
                    rotationY = 0.0f;
                    InfoShowSelect();
                    currentCharacter.GetComponent<Animator>().Play(playerStandsManager.GetAnimationNameOnNotSelected(playerHandle));
                }
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

        #region Info

        private void InfoShowJoin()
        {
            infoJoin.SetActive(true);
            infoSelect.SetActive(false);
            infoUnselect.SetActive(false);
        }

        private void InfoShowSelect()
        {
            infoJoin.SetActive(false);
            infoSelect.SetActive(true);
            infoUnselect.SetActive(false);
        }

        private void InfoShowUnselect()
        {
            infoJoin.SetActive(false);
            infoSelect.SetActive(false);
            infoUnselect.SetActive(true);
        }

        #endregion
    }
}