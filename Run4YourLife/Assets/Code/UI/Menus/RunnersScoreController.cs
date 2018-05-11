using UnityEngine;
using UnityEngine.UI;

using Run4YourLife.Player;
using Run4YourLife.Utils;

namespace Run4YourLife.UI
{
    public class RunnersScoreController : MonoBehaviour
    {
        [SerializeField]
        RunnerScoreController[] m_runnerScoreControllers;

        private void Start()
        {
            StartCoroutine(YieldHelper.SkipFrame(InitializeScores));
        }

        private void InitializeScores()
        {
            for (int i = 0; i < PlayerManager.Instance.RunnerPlayerHandles.Count; i++)
            {
                PlayerHandle playerHandle = PlayerManager.Instance.RunnerPlayerHandles[i];
                m_runnerScoreControllers[i].gameObject.SetActive(true);
                m_runnerScoreControllers[i].SetplayerHandle(playerHandle);
            }
        }
    }
}