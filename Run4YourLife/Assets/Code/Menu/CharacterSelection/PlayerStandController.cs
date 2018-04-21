using UnityEngine;

using Run4YourLife.Input;
using Run4YourLife.Player;

namespace Run4YourLife.CharacterSelection
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

        protected bool ready = false;

        protected float rotationY = 0.0f;

        #region Stands

        protected GameObject activeStand;

        #endregion

        #region References

        protected PlayerHandle playerDefinition;
        protected PlayerStandControllerControlScheme controlScheme;

        #endregion

        void Awake()
        {
            controlScheme = GetComponent<PlayerStandControllerControlScheme>();
            Debug.Assert(controlScheme != null);

            OnAwake();
        }

        void Update()
        {
            if(playerDefinition != null)
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

        public PlayerHandle GetPlayerDefinition()
        {
            return playerDefinition;
        }

        public void OnPlayerDefinitionChanged(PlayerHandle playerDefinition)
        {
            if(playerDefinition == null)
            {
                ClearPlayerDefinition();
            }
            else
            {
                SetPlayerDefinition(playerDefinition);
            }
        }

        protected void SetPlayerDefinition(PlayerHandle playerDefinition)
        {
            this.playerDefinition = playerDefinition;
            activeStand = SpawnPlayerStand(playerDefinition);
            controlScheme.Active = true;
            ui.SetActive(true);
        }

        protected void ClearPlayerDefinition()
        {
            playerDefinition = null;
            Destroy(activeStand);
            activeStand = null;
            controlScheme.Active = false;
            ui.SetActive(false);
        }

        private GameObject SpawnPlayerStand(PlayerHandle playerDefinition)
        {
            GameObject prefab = GetStandPrefabForPlayer(playerDefinition);
            GameObject instance = Instantiate(prefab, spawnTransform, false);

            return instance;
        }

        protected abstract GameObject GetStandPrefabForPlayer(PlayerHandle playerDefinition);

        protected virtual void UpdatePlayer()
        {
            if(controlScheme.Ready.Started())
            {
                if(ready)
                {
                    ready = false;
                    readyTextScaleTick.gameObject.SetActive(false);
                    OnNotReady();
                }
                else
                {
                    ready = true;
                    readyBoxScaleTick.Tick();
                    readyTextScaleTick.Tick();
                    readyTextScaleTick.gameObject.SetActive(true);
                    OnReady();
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