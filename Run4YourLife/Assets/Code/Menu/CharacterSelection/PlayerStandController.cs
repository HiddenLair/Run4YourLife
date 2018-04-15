using UnityEngine;

using Run4YourLife.Input;
using Run4YourLife.Player;

namespace Run4YourLife.CharacterSelection
{
    public abstract class PlayerStandController : MonoBehaviour, IPlayerDefinitionEvents
    {
        #region References

        protected PlayerStandsManager playerStandsManager;
        protected PlayerDefinition playerDefinition;
        protected PlayerManager playerManager;
        protected PlayerStandControllerControlScheme controlScheme;

        #endregion

        #region Stands

        protected GameObject activeStand;

        #endregion

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

        void Awake()
        {
            playerStandsManager = FindObjectOfType<PlayerStandsManager>();
            Debug.Assert(playerStandsManager != null);

            playerManager = FindObjectOfType<PlayerManager>();
            Debug.Assert(playerManager != null);

            controlScheme = GetComponent<PlayerStandControllerControlScheme>();
            Debug.Assert(controlScheme != null);
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

        public PlayerDefinition GetPlayerDefinition()
        {
            return playerDefinition;
        }

        public void OnPlayerDefinitionChanged(PlayerDefinition playerDefinition)
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

        protected void SetPlayerDefinition(PlayerDefinition playerDefinition)
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

        private GameObject SpawnPlayerStand(PlayerDefinition playerDefinition)
        {
            GameObject prefab = GetStandPrefabForPlayer(playerDefinition);
            GameObject instance = Instantiate(prefab, spawnTransform, false);

            return instance;
        }

        protected abstract GameObject GetStandPrefabForPlayer(PlayerDefinition playerDefinition);

        protected virtual void UpdatePlayer()
        {
            if(controlScheme.Ready.Started())
            {
                if(ready)
                {
                    ready = false;
                    readyTextScaleTick.gameObject.SetActive(false);
                }
                else
                {
                    ready = true;
                    readyBoxScaleTick.Tick();
                    readyTextScaleTick.Tick();
                    readyTextScaleTick.gameObject.SetActive(true);
                }
            }
            else if(controlScheme.Exit.Started())
            {
                playerStandsManager.GoMainMenu();
            }
            else if(Mathf.Abs(controlScheme.Rotate.Value()) > 0.5f)
            {
                rotationY += -200.0f * controlScheme.Rotate.Value() * Time.deltaTime;
            }
        }
    }
}