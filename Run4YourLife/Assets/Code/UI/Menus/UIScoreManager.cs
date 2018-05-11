using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Run4YourLife.Player;
using Run4YourLife.Utils;

namespace Run4YourLife.UI
{
    public interface IUIScoreEvents : IEventSystemHandler
    {
        void OnScoreChanged(PlayerHandle playerHandle, float score);
    }

    public class UIScoreManager : MonoBehaviour, IUIScoreEvents
    {
        [SerializeField]
        private RunnerScoreController[] m_runnerScoreControllers;

        private Dictionary<PlayerHandle, RunnerScoreController> m_playerHandleRunnerScoreControllers = new Dictionary<PlayerHandle, RunnerScoreController>();

        private void Start()
        {
            StartCoroutine(YieldHelper.SkipFrame(InitializeScores));
        }

        private void InitializeScores()
        {
            for (int i = 0; i < PlayerManager.Instance.RunnerPlayerHandles.Count; i++)
            {
                PlayerHandle playerHandle = PlayerManager.Instance.RunnerPlayerHandles[i];
                RunnerScoreController runnerScoreController = m_runnerScoreControllers[i];
                m_playerHandleRunnerScoreControllers.Add(playerHandle, runnerScoreController);

                runnerScoreController.gameObject.SetActive(true);
            }
        }

        public void OnScoreChanged(PlayerHandle playerHandle, float score)
        {
            m_playerHandleRunnerScoreControllers[playerHandle].SetScore(score);
        }
    }
}