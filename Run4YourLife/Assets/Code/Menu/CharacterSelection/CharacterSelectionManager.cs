using System;
using UnityEngine;

using Run4YourLife.Input;
using Run4YourLife.Player;
using Run4YourLife.SceneManagement;

namespace Run4YourLife.CharacterSelection
{
    [RequireComponent(typeof(ControllerDetector))]
    public class CharacterSelectionManager : SingletonMonoBehaviour<CharacterSelectionManager>
    {
        [SerializeField]
        private GameObject m_gameLoadRequest;

        [SerializeField]
        private SceneTransitionRequest m_mainMenuLoadRequest;

        void Awake()
        {
            ControllerDetector controllerDetector = GetComponent<ControllerDetector>();
            Debug.Assert(controllerDetector != null);
            controllerDetector.OncontrollerDetected.AddListener(OnControllerDetected);
        }

        private void Start()
        {
            PlayerManager.Instance.ClearPlayers();
        }

        public void OnControllerDetected(InputDevice controller)
        {
            if(PlayerManager.Instance.GetPlayers().Count < 4)
            {
                CreatePlayerForController(controller);
            }
        }

        private void CreatePlayerForController(InputDevice inputDevice)
        {
            PlayerHandle playerDefinition = new PlayerHandle();

            playerDefinition.inputDevice = inputDevice;
            if(PlayerManager.Instance.GetPlayers().Count == 0)
            {
                PlayerManager.Instance.SetPlayerAsBoss(playerDefinition);
            }
            playerDefinition.CharacterType = GetFirstAviablePlayerCharacterType();
            playerDefinition.ID = inputDevice.ID;

            PlayerManager.Instance.AddPlayer(playerDefinition);
        }

        private CharacterType GetFirstAviablePlayerCharacterType()
        {
            CharacterType ret = CharacterType.Purple;
            foreach(CharacterType characterType in Enum.GetValues(typeof(CharacterType)))
            {
                if(!PlayerHasCharacterType(characterType))
                {
                    ret = characterType;
                    break;
                }
            }
            return ret;
        }

        private bool PlayerHasCharacterType(CharacterType characterType)
        {
            foreach(PlayerHandle player in PlayerManager.Instance.GetPlayers())
            {
                if(player.CharacterType.Equals(characterType))
                {
                    return true;
                }
            }
            return false;
        }

        public void OnGameStart()
        {
            foreach(SceneTransitionRequest request in m_gameLoadRequest.GetComponents<SceneTransitionRequest>())
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