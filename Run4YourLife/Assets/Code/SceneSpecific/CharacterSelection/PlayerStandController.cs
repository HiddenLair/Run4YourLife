using UnityEngine;

using Run4YourLife.Utils;
using Run4YourLife.Player;
using Run4YourLife.GameManagement;
using Run4YourLife.InputManagement;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    [RequireComponent(typeof(PlayerStandControllerControlScheme))]
    public class PlayerStandController : MonoBehaviour, IPlayerHandleEvent
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

        [SerializeField]
        private GameObject infoReady;

        #region Audio

        [SerializeField]
        private AudioClip audioOnSelectCompletedIfBoss;

        [SerializeField]
        private AudioClip audioOnSelectCompletedIfRunner;

        [SerializeField]
        private AudioClip audioOnSelectError;

        [SerializeField]
        private AudioClip audioOnUnselectCompleted;

        [SerializeField]
        private AudioClip audioOnUnselectError;

        [SerializeField]
        private AudioClip audioOnMoveCompleted;

        [SerializeField]
        private AudioClip audioOnMoveError;

        #endregion

        private PlayerHandle playerHandle;
        private PlayerStandsManager playerStandsManager;
        private PlayerStandControllerControlScheme playerStandControlScheme;

        private GameObject currentCharacter;
        private GameObject currentCharacterPrefab;

        private bool checkVerticalPlayerInput = true;
        private bool checkHorizontalPlayerInput = true;

        private float rotationY = 0.0f;

        public PlayerHandle PlayerHandle { get { return playerHandle; } }

        void Awake()
        {
            playerStandsManager = PlayerStandsManager.Instance;
            playerStandControlScheme = GetComponent<PlayerStandControllerControlScheme>();

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
                    currentCharacter.SetActive(false);
                }

                currentCharacter = DynamicObjectsManager.Instance.GameObjectPool.GetAndPositionAndScale(characterPrefab, spawnPosition.position, spawnPosition.rotation, spawnPosition.localScale, true);
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
                    InfoShowUnselect();
                    currentCharacter.GetComponent<Animator>().Play(playerStandsManager.GetAnimationNameOnSelected(playerHandle));
                    currentCharacter.GetComponent<ScaleOnTick>().Tick();
                }

                PlaySfx(playerStandsManager.IsOnBossCell(playerHandle) ? audioOnSelectCompletedIfBoss : audioOnSelectCompletedIfRunner, audioOnSelectError, requestCompletionState);
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

                PlaySfx(audioOnUnselectCompleted, audioOnUnselectError, requestCompletionState);
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
                    PlaySfx(audioOnMoveCompleted, audioOnMoveError, playerStandsManager.OnPlayerInputUp(playerHandle));
                }
                else if(currentVertical <= -threshold)
                {
                    checkVerticalPlayerInput = false;
                    PlaySfx(audioOnMoveCompleted, audioOnMoveError, playerStandsManager.OnPlayerInputDown(playerHandle));
                }
            }

            if(checkHorizontalPlayerInput)
            {
                if(currentHorizontal >= threshold)
                {
                    checkHorizontalPlayerInput = false;
                    PlaySfx(audioOnMoveCompleted, audioOnMoveError, playerStandsManager.OnPlayerInputRight(playerHandle));
                }
                else if(currentHorizontal <= -threshold)
                {
                    checkHorizontalPlayerInput = false;
                    PlaySfx(audioOnMoveCompleted, audioOnMoveError, playerStandsManager.OnPlayerInputLeft(playerHandle));
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

            rotationY += rotationSpeed * playerStandControlScheme.Rotate.Value() * Time.deltaTime;

            #endregion
        }

        private void PlaySfx(AudioClip audioCompleted, AudioClip audioError, RequestCompletionState requestCompletionState)
        {
            AudioClip audioClip = null;

            if(requestCompletionState == RequestCompletionState.Completed)
            {
                audioClip = audioCompleted;
            }
            else if(requestCompletionState == RequestCompletionState.Error)
            {
                audioClip = audioError;
            }

            if(audioClip != null)
            {
                AudioManager.Instance.PlaySFX(audioClip);
            }
        }

        #region Info

        private void InfoShowJoin()
        {
            infoJoin.SetActive(true);
            infoSelect.SetActive(false);
            infoUnselect.SetActive(false);
            infoReady.SetActive(false);
        }

        private void InfoShowSelect()
        {
            infoJoin.SetActive(false);
            infoSelect.SetActive(true);
            infoUnselect.SetActive(false);
            infoReady.SetActive(false);
        }

        private void InfoShowUnselect()
        {
            infoJoin.SetActive(false);
            infoSelect.SetActive(false);
            infoUnselect.SetActive(true);
            infoReady.SetActive(true);
        }

        #endregion
    }
}