using UnityEngine;

using Run4YourLife.InputManagement;
using Run4YourLife.Player;
using Run4YourLife.SceneManagement;

namespace Run4YourLife.SceneSpecific.CharacterSelection
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
            PlayerHandle playerHandle = new PlayerHandle
            {
                InputDevice = inputDevice,
                CharacterType = PlayerManager.Instance.GetFirstAviablePlayerCharacterType(),
                IsBoss = PlayerManager.Instance.BossPlayerHandle == null
            };

            PlayerManager.Instance.AddPlayer(playerHandle);        
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