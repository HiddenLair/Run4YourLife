using System;
using UnityEngine;

using Run4YourLife.Input;
using Run4YourLife.Player;
using Run4YourLife.SceneManagement;

namespace Run4YourLife.CharacterSelection
{
    [RequireComponent(typeof(InputDeviceDetector))]
    public class CharacterSelectionManager : SingletonMonoBehaviour<CharacterSelectionManager>
    {
        [SerializeField]
        private SceneTransitionRequest m_gameLoadRequest;

        [SerializeField]
        private SceneTransitionRequest m_mainMenuLoadRequest;

        void Awake()
        {
            InputDeviceDetector controllerDetector = GetComponent<InputDeviceDetector>();
            controllerDetector.OncontrollerDetected.AddListener(OnControllerDetected);
        }

        private void Start()
        {
            PlayerManager.Instance.ClearPlayers();
        }

        public void OnControllerDetected(InputDevice controller)
        {
            if(PlayerManager.Instance.PlayerHandles.Count < 4)
            {
                CreatePlayerForController(controller);
            }
        }

        private void CreatePlayerForController(InputDevice inputDevice)
        {
            PlayerHandle playerDefinition = new PlayerHandle
            {
                inputDevice = inputDevice,
                CharacterType = PlayerManager.Instance.GetFirstAviablePlayerCharacterType(),
                ID = inputDevice.ID
            };

            if (PlayerManager.Instance.PlayerHandles.Count == 0)
            {
                PlayerManager.Instance.SetPlayerAsBoss(playerDefinition);
            }

            PlayerManager.Instance.AddPlayer(playerDefinition);
        }

        public void OnGameStart()
        {
            m_gameLoadRequest.Execute();
        }

        public void OnMainMenuStart()
        {
            m_mainMenuLoadRequest.Execute();
        }
    }
}