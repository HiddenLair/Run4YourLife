using UnityEngine;
using UnityEngine.UI;

using Run4YourLife.Player;
using Run4YourLife.Utils;

namespace Run4YourLife.UI
{
    [RequireComponent(typeof(HorizontalLayoutGroup)]
    public class RunnersScoreController : MonoBehaviour
    {
        [SerializeField]
        private GameObject scoreTextPrefab;

        private HorizontalLayoutGroup scoreTexts;

        private void Awake()
        {
            scoreTexts = GetComponent<HorizontalLayoutGroup>();
        }

        private void Start()
        {
            StartCoroutine(YieldHelper.SkipFrame(InitializeScores));
        }

        private void InitializeScores()
        {
            foreach (PlayerHandle playerHandle in PlayerManager.Instance.RunnerPlayerHandles)
            {
                GameObject scoreText = Instantiate(scoreTextPrefab, scoreTexts.transform, false);
                scoreText.GetComponent<RunnerScoreController>().SetplayerHandle(playerHandle);
            }
        }
    }
}