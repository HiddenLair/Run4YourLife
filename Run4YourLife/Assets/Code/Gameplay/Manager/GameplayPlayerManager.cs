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

        #region Members

        private Queue<PlayerDefinition> m_deadPlayers = new Queue<PlayerDefinition>();

        private PlayerManager m_playerManager;

        #endregion

        public GameObject Boss { get; set; }

        private List<GameObject> m_runners = new List<GameObject>();
        public List<GameObject> Runners { get { return m_runners; } set { m_runners = value; } }

        void Start() {
            m_playerManager = FindObjectOfType<PlayerManager>();
            Debug.Assert(m_playerManager != null);
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
            throw new System.NotImplementedException();
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
