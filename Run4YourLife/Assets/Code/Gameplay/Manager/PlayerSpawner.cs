using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;

namespace Run4YourLife.GameManagement
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_playerPrefab;

        [SerializeField]
        private GameObject m_bossPrefab;

        [SerializeField]
        private Transform[] m_runnerSpawns;

        [SerializeField]
        private Transform m_bossSpawn;

        private PlayerManager m_playerManager;
        private GameplayPlayerManager m_gameplayPlayerManager;

        private void Awake()
        {
            m_gameplayPlayerManager = FindObjectOfType<GameplayPlayerManager>();
            Debug.Assert(m_gameplayPlayerManager != null);
        }

        private void Start()
        {
            m_playerManager = FindObjectOfType<PlayerManager>();
            Debug.Assert(m_playerManager != null);
        }

        public GameObject[] InstantiatePlayers()
        {
            GameObject[] players = new GameObject[m_playerManager.GetPlayers().Count];
            uint index = 0;

            foreach (PlayerDefinition playerDefinition in m_playerManager.GetPlayers())
            {
                players[index] = InstantiatePlayer(playerDefinition);
                index++;
            }

            return players;
        }

        public GameObject InstantiateBossPlayer()
        {
            PlayerDefinition bossPlayerDefinition = m_playerManager.GetPlayers().Find(x => x.IsBoss);
            Debug.Assert(bossPlayerDefinition != null);

            return InstantiatePlayer(bossPlayerDefinition);
        }

        private GameObject InstantiatePlayer(PlayerDefinition playerDefinition)
        {
            GameObject player = InstantiatePlayerByType(playerDefinition);
            ExecuteEvents.Execute<IPlayerDefinitionEvents>(player, null, (a, b) => a.OnPlayerDefinitionChanged(playerDefinition));
            return player;
        }

        private GameObject InstantiatePlayerByType(PlayerDefinition playerDefinition)
        {
            GameObject instance;

            if (playerDefinition.IsBoss)
            {
                instance = Instantiate(m_bossPrefab, m_bossSpawn.position, m_bossSpawn.rotation);
                m_gameplayPlayerManager.Boss = instance;
            }
            else
            {
                // TODO implementation of spawn that requires 3 transforms instead of 4
                Transform spawn = m_runnerSpawns[playerDefinition.ID - 1];
                instance = Instantiate(m_playerPrefab, spawn.position, spawn.rotation);
                m_gameplayPlayerManager.Runners.Add(instance);
            }

            return instance;
        }
    }
}

