using UnityEngine;

using Run4YourLife.InputManagement;
using Run4YourLife.Player;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    public abstract class PlayerStandController : MonoBehaviour, IPlayerHandleEvent
    {
        [SerializeField]
        protected GameObject ui;

        [SerializeField]
        protected ScaleTick readyBoxScaleTick;

        [SerializeField]
        protected ScaleTick readyTextScaleTick;

        [SerializeField]
        protected Transform spawnTransform;

        [SerializeField]
        private float scaleMultiplierReady = 1.1f;

        protected bool ready = false;

        protected float rotationY = 0.0f;

        private Vector3 initialScale;

        #region Stands

        protected GameObject activeStand;

        #endregion

        #region References

        protected PlayerHandle playerHandle;
        protected PlayerStandControllerControlScheme controlScheme;

        #endregion

        void Awake()
        {
            controlScheme = GetComponent<PlayerStandControllerControlScheme>();
            Debug.Assert(controlScheme != null);

            initialScale = transform.localScale;

            OnAwake();
        }

        void Update()
        {
            if(playerHandle != null)
            {
                UpdatePlayer();
            }
            else
            {
                rotationY = 0.0f;
            }

            if(activeStand != null)
            {
                activeStand.transform.rotation = Quaternion.AngleAxis(rotationY, Vector3.up);
            }
        }

        public bool GetReady()
        {
            return ready;
        }

        public PlayerHandle GetplayerHandle()
        {
            return playerHandle;
        }

        public void OnPlayerHandleChanged(PlayerHandle playerHandle)
        {
            if(playerHandle == null)
            {
                ClearplayerHandle();
            }
            else
            {
                SetplayerHandle(playerHandle);
            }
        }

        protected void SetplayerHandle(PlayerHandle playerHandle)
        {
            this.playerHandle = playerHandle;
            activeStand = SpawnPlayerStand(playerHandle);
            controlScheme.Active = true;
            ui.SetActive(true);
        }

        protected void ClearplayerHandle()
        {
            playerHandle = null;
            Destroy(activeStand);
            activeStand = null;
            controlScheme.Active = false;
            ui.SetActive(false);
        }

        private GameObject SpawnPlayerStand(PlayerHandle playerHandle)
        {
            GameObject prefab = GetStandPrefabForPlayer(playerHandle);
            GameObject instance = Instantiate(prefab, spawnTransform, false);

            return instance;
        }

        protected abstract GameObject GetStandPrefabForPlayer(PlayerHandle playerHandle);

        protected virtual void UpdatePlayer()
        {
            if(controlScheme.Ready.Started())
            {
                if(ready)
                {
                    ready = false;
                    readyTextScaleTick.gameObject.SetActive(false);
                    OnNotReady();
                    transform.localScale = initialScale;
                }
                else
                {
                    ready = true;
                    readyBoxScaleTick.Tick();
                    readyTextScaleTick.Tick();
                    readyTextScaleTick.gameObject.SetActive(true);
                    OnReady();
                    transform.localScale = scaleMultiplierReady * initialScale;
                }
            }
            else if(controlScheme.Exit.Started())
            {
                PlayerStandsManager.Instance.GoMainMenu();
            }
            else if(Mathf.Abs(controlScheme.Rotate.Value()) > 0.5f)
            {
                rotationY += -200.0f * controlScheme.Rotate.Value() * Time.deltaTime;
            }
        }

        protected virtual void OnAwake() { }

        protected virtual void OnReady() { }

        protected virtual void OnNotReady() { }
    }
}