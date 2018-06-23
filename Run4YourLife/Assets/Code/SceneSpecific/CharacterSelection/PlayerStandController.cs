using UnityEngine;
using UnityEngine.Playables;

using Run4YourLife.InputManagement;
using Run4YourLife.Player;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    public abstract class PlayerStandController : MonoBehaviour, IPlayerHandleEvent
    {
        [SerializeField]
        private float m_rotationSpeed;        

        [SerializeField]
        private Transform m_spawn;

        [SerializeField]
        private float scaleMultiplierReady = 1.1f;

        [SerializeField]
        private PlayableDirector m_readyPlayableDirector;
        
        [SerializeField]
        private GameObject m_readyText;

        public bool IsReady { get { return m_ready; } }
        public PlayerHandle PlayerHandle { get { return m_playerHandle; } }

        private bool m_ready;

        protected float rotationY = 0.0f;

        private Vector3 initialScale;

        private GameObject m_ui;
        protected Transform activeStand;
        private PlayerHandle m_playerHandle;
        protected PlayerStandControllerControlScheme m_playerStandControlScheme;

        protected virtual void Awake()
        {
            m_ui = transform.Find("UI").gameObject;
            Debug.Assert(m_ui != null);

            m_playerStandControlScheme = GetComponent<PlayerStandControllerControlScheme>();
            Debug.Assert(m_playerStandControlScheme != null);

            initialScale = transform.localScale;
        }

        public void OnPlayerHandleChanged(PlayerHandle playerHandle)
        {
            m_playerHandle = playerHandle;

            bool hasPlayerHandle = playerHandle != null;
            m_ui.SetActive(hasPlayerHandle);
            m_playerStandControlScheme.Active = hasPlayerHandle;

            if(activeStand != null)
            {
                Destroy(activeStand.gameObject);
                activeStand = null;
            }

            if(hasPlayerHandle)
            {
                activeStand = SpawnPlayerStand(playerHandle).transform;
            }
            else
            {
                rotationY = 0.0f;
            }
        }

        private void Update()
        {
            if(m_playerHandle != null)
            {
                UpdatePlayer();

                if(activeStand != null)
                {
                    activeStand.rotation = Quaternion.AngleAxis(rotationY, Vector3.up);
                }
            }
        }

        private GameObject SpawnPlayerStand(PlayerHandle playerHandle)
        {
            GameObject prefab = GetStandPrefabForPlayer(playerHandle);
            GameObject instance = Instantiate(prefab, m_spawn, false);

            return instance;
        }

        protected abstract GameObject GetStandPrefabForPlayer(PlayerHandle playerHandle);

        protected virtual void UpdatePlayer()
        {
            rotationY += m_rotationSpeed * m_playerStandControlScheme.Rotate.Value() * Time.deltaTime;

            if(m_playerStandControlScheme.Ready.Started())
            {
                m_ready = !m_ready;

                if(!m_ready)
                {
                    m_readyText.SetActive(false);

                    transform.localScale = initialScale;

                    OnNotReady();
                }
                else
                {
                    m_readyText.SetActive(true);
                    m_readyPlayableDirector.Play();
                    
                    transform.localScale = scaleMultiplierReady * initialScale;

                    OnReady();
                }
            }
        }

        protected abstract void OnReady();

        protected abstract void OnNotReady();
    }
}