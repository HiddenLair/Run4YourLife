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

    public class PlayerStateManager : MonoBehaviour, IPlayerStateEvents {

        #region Editor

        [SerializeField]
        private UnityEvent onAllPlayersDeath;

        #endregion
        #region Members

        private Queue<PlayerDefinition> deadPlayers = new Queue<PlayerDefinition>();

        private PlayerManager playerManager;

        #endregion

        // Use this for initialization
        void Start() {
            playerManager = FindObjectOfType<PlayerManager>();
        }

        // Update is called once per frame
        void Update() {

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
