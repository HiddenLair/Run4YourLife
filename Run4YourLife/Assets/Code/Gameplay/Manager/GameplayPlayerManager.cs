using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Run4YourLife.Player;

namespace Run4YourLife.GameManagement {
    public interface IGameplayPlayerEvents : IEventSystemHandler
    {
        void OnRunnerDeath(GameObject player);
        void OnPlayerReviveRequest(Vector3 position);
    }

    public class GameplayPlayerManager : MonoBehaviour, IGameplayPlayerEvents {

        #region Editor

        [SerializeField]
        private UnityEvent m_onAllRunnersDied;

        #endregion

        #region Members and properties

        private Queue<PlayerDefinition> m_deadPlayers = new Queue<PlayerDefinition>();

        private PlayerSpawner playerSpawner;

        private PlayerManager m_playerManager;
        public PlayerManager PlayerManager
        {
            get
            {
                if (m_playerManager == null)
                {
                    m_playerManager = GetOrCreateDefaultPlayerManagerIfNoneIsAviable();
                    Debug.Assert(m_playerManager != null);
                }
                return m_playerManager;
            }
        }

        public GameObject Boss { get; set; }

        private List<GameObject> m_runners = new List<GameObject>();
        public List<GameObject> Runners { get { return m_runners; } set { m_runners = value; } }

        #endregion

        private void Awake()
        {
            playerSpawner = GetComponent<PlayerSpawner>();
        }

        private PlayerManager GetOrCreateDefaultPlayerManagerIfNoneIsAviable()
        {
            //TODO if no playermanager is found, create default player manager
            //useful for debug opening the scene+
            PlayerManager playerManager = FindObjectOfType<PlayerManager>();
            if (playerManager == null)
            {
                playerManager = gameObject.AddComponent<PlayerManager>();
                playerManager.AddPlayer(new PlayerDefinition()
                {
                    CharacterType = CharacterType.Red,
                    ID = 1,
                    inputDevice = new Input.InputDevice(1),
                    IsBoss = false
                });
                playerManager.AddPlayer(new PlayerDefinition()
                {
                    CharacterType = CharacterType.Orange,
                    ID = 2,
                    inputDevice = new Input.InputDevice(2),
                    IsBoss = true
                });
                playerManager.AddPlayer(new PlayerDefinition()
                {
                    CharacterType = CharacterType.Green,
                    ID = 3,
                    inputDevice = new Input.InputDevice(3),
                    IsBoss = false
                });
                playerManager.AddPlayer(new PlayerDefinition()
                {
                    CharacterType = CharacterType.Blue,
                    ID = 4,
                    inputDevice = new Input.InputDevice(4),
                    IsBoss = false
                });
            }
            return playerManager;
        }

        public void OnRunnerDeath(GameObject player)
        {
            Runners.Remove(player);
            
            PlayerDefinition playerDefinition = player.GetComponent<PlayerInstance>().PlayerDefinition;
            m_deadPlayers.Enqueue(playerDefinition);

            Destroy(player);

            if (m_playerManager.GetPlayers().Count == m_deadPlayers.Count + 1)
            {
                m_onAllRunnersDied.Invoke();
            }
        }

        public void OnPlayerReviveRequest(Vector3 position)
        {
            PlayerDefinition playerToRevive = m_deadPlayers.Dequeue();
            playerSpawner.InstantiateRunner(playerToRevive,position);       
        }

        public void ReviveAllPlayers()
        {
            //TODO: fill the method
        }

        public void DebugDestroyAllPlayersAndClear()
        {
            Destroy(Boss);
            foreach(GameObject runner in Runners)
            {
                Destroy(runner);
            }

            m_deadPlayers.Clear();
        }
    }
}
