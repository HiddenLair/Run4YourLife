using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Run4YourLife.Player;

namespace Run4YourLife.GameManagement {
    public interface IPlayerStateEvents : IEventSystemHandler
    {
        void OnPlayerDeath(PlayerDefinition player);
        void OnPlayerReviveRequest(Vector3 position);
    }

    public class GameplayPlayerManager : MonoBehaviour, IPlayerStateEvents {

        #region Editor

        [SerializeField]
        private UnityEvent onAllPlayersDeath;

        #endregion
        #region Members

        private Queue<PlayerDefinition> deadPlayers = new Queue<PlayerDefinition>();

        private PlayerManager playerManager;

        #endregion

        void Start() {
            playerManager = FindObjectOfType<PlayerManager>();
        }

        public void OnPlayerDeath(PlayerDefinition player)
        {
            deadPlayers.Enqueue(player);
            if (playerManager.GetPlayers().Count == deadPlayers.Count + 1)
            {
                onAllPlayersDeath.Invoke();
            }
        }

        public void OnPlayerReviveRequest(Vector3 position)
        {
            throw new System.NotImplementedException();
        }
    }
}
