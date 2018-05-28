using TMPro;
using UnityEngine;
using UnityEngine.Playables;

using Run4YourLife.GameManagement;
using Run4YourLife.InputManagement;

namespace Run4YourLife.Player
{
    public class RunnerHUDController : MonoBehaviour, IPlayerHandleEvent, IRunnerScoreEvents
    {
        private static Color32 BLUE_RUNNER_COLOR = Color.blue;
        private static Color32 GREEN_RUNNER_COLOR = Color.green;
        private static Color32 PURPLE_RUNNER_COLOR = new Color32(128, 0, 128, 255);
        private static Color32 ORANGE_RUNNER_COLOR = new Color32(255, 127, 80, 255);

        [SerializeField]
        private TextMeshPro text;

        [SerializeField]
        private PlayableDirector playableDirector;

        void Start()
        {
            text.text = "0";
        }

        public void OnPlayerHandleChanged(PlayerHandle playerHandle)
        {
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

        public void OnScoreChanged(int score)
        {
            text.text = score.ToString();
            playableDirector.Play();
        }
    }
}