using TMPro;
using UnityEngine;
using UnityEngine.Playables;

using Run4YourLife.InputManagement;

namespace Run4YourLife.Player
{
    public class RunnerHUDController : MonoBehaviour, IPlayerHandleEvent
    {
        private static Color32 BLUE_RUNNER_COLOR = Color.blue;
        private static Color32 GREEN_RUNNER_COLOR = Color.green;
        private static Color32 PURPLE_RUNNER_COLOR = new Color32(128, 0, 128, 255);
        private static Color32 ORANGE_RUNNER_COLOR = new Color32(255, 127, 80, 255);

        [SerializeField]
        private TextMeshPro text;

        [SerializeField]
        private PlayableDirector playableDirector;

        private InputController inputController;
        private RunnerControlScheme runnerControlScheme;

        void Awake()
        {
            inputController = GetComponent<InputController>();
            runnerControlScheme = GetComponent<RunnerControlScheme>();
        }

        void Update()
        {
            if(inputController.Started(runnerControlScheme.Focus))
            {
                playableDirector.Play();
            }
        }

        public void OnPlayerHandleChanged(PlayerHandle playerHandle)
        {
            text.text = playerHandle.ID.ToString();

            switch(playerHandle.CharacterType)
            {
                case CharacterType.Blue:
                    text.color = BLUE_RUNNER_COLOR;
                    break;
                case CharacterType.Green:
                    text.color = GREEN_RUNNER_COLOR;
                    break;
                case CharacterType.Purple:
                    text.color = PURPLE_RUNNER_COLOR;
                    break;
                case CharacterType.Orange:
                    text.color = ORANGE_RUNNER_COLOR;
                    break;
            }
        }
    }
}