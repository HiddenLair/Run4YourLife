using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.Input;
using System;

using Run4YourLife.SceneManagement;

namespace Run4YourLife.CharacterSelection
{
    [RequireComponent(typeof(ControllerDetector))]
    public class CharacterSelectionManager : MonoBehaviour
    {
        private PlayerManager m_playerManager;

        [SerializeField]
        private GameObject m_gameLoadRequest;

        [SerializeField]
        private SceneLoadRequest m_mainMenuLoadRequest;

        void Awake()
        {
            m_playerManager = Component.FindObjectOfType<PlayerManager>();
            Debug.Assert(m_playerManager != null);

            ControllerDetector controllerDetector = GetComponent<ControllerDetector>();
            Debug.Assert(controllerDetector != null);
            controllerDetector.OncontrollerDetected.AddListener(OnControllerDetected);
        }

        public void OnControllerDetected(InputDevice controller)
        {
            CreatePlayerForController(controller);
        }

        private void CreatePlayerForController(InputDevice inputDevice)
        {
            PlayerDefinition playerDefinition = new PlayerDefinition();

            playerDefinition.inputDevice = inputDevice;
            if (m_playerManager.GetPlayers().Count == 0)
            {
                m_playerManager.SetPlayerAsBoss(playerDefinition);
            }
            playerDefinition.CharacterType = GetFirstAviablePlayerCharacterType();
            playerDefinition.ID = inputDevice.ID;

            m_playerManager.AddPlayer(playerDefinition);
        }

        private CharacterType GetFirstAviablePlayerCharacterType()
        {
            CharacterType ret = CharacterType.Red;
            foreach (CharacterType characterType in Enum.GetValues(typeof(CharacterType)))
            {
                if (!PlayerHasCharacterType(characterType))
                {
                    ret = characterType;
                    break;
                }
            }
            return ret;
        }

        private bool PlayerHasCharacterType(CharacterType characterType)
        {
            foreach(PlayerDefinition player in m_playerManager.GetPlayers())
            {
                if (player.CharacterType.Equals(characterType))
                {
                    return true;
                }
            }
            return false;
        }

        public void OnGameStart()
        {
            foreach (SceneLoadRequest request in m_gameLoadRequest.GetComponents<SceneLoadRequest>())
            {
                request.Execute();
            }
        }

        public void OnMainMenuStart()
        {
            m_mainMenuLoadRequest.Execute();
        }
    }
}