using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.UI;

namespace Run4YourLife.GameManagement
{
    public interface IProgressProvider
    {
        float Progress { get; }
    }

    public class ProgressManager : MonoBehaviour
    {
        private GameObject m_uiManager;

        void Awake()
        {
            m_uiManager = GameObject.FindGameObjectWithTag(Tags.UI);
            Debug.Assert(m_uiManager != null, "UI not found");
        }

        private void Start()
        {
            GameManager.Instance.onGamePhaseChanged.AddListener(OnGamePhaseChanged);
        }

        private void OnDestroy()
        {
            GameManager.Instance.onGamePhaseChanged.RemoveListener(OnGamePhaseChanged);
        }

        void Update()
        {
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            if (boss != null)
            {
                IProgressProvider progressProvider = boss.GetComponent<IProgressProvider>();
                ExecuteEvents.Execute<IUIEvents>(m_uiManager, null, (x, y) => x.OnBossProgress(progressProvider.Progress));
            }
        }

        private void OnGamePhaseChanged(GamePhase gamePhase)
        {
            switch(gamePhase)
            {
                case GamePhase.EasyMoveHorizontal:
                case GamePhase.HardMoveHorizontal:
                    enabled = true;
                    break;
                case GamePhase.BossFight:
                case GamePhase.BossFightRockTransition:
                case GamePhase.End:
                case GamePhase.TransitionToBossFight:
                case GamePhase.TransitionToEasyMoveHorizontal:
                case GamePhase.TransitionToHardMoveHorizontal:
                    enabled = false;
                    break;
            }
        }
    }
}