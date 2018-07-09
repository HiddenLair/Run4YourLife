using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.GameManagement;

namespace Run4YourLife.UI
{
    public interface IUIProgressEvents : IEventSystemHandler
    {
        void OnBossProgress(float percent);
    }

    public class UIProgress : SingletonMonoBehaviour<UIProgress>, IUIProgressEvents
    {
        [SerializeField]
        private GameObject progressUI;

        private Progress progress;
        private CanvasGroup progressCanvasGroup;

        private void Awake()
        {
            progress = progressUI.GetComponent<Progress>();
            Debug.Assert(progress != null);

            progressCanvasGroup = progressUI.GetComponent<CanvasGroup>();
            Debug.Assert(progressCanvasGroup != null);

            GameManager.Instance.onGamePhaseChanged.AddListener(OnGamePhaseChanged);
        }

        private void OnGamePhaseChanged(GamePhase gamePhase)
        {
            progress.SetPhase(gamePhase);

            if(gamePhase == GamePhase.EasyMoveHorizontal || gamePhase == GamePhase.HardMoveHorizontal)
            {
                ShowProgress();
            }
            else
            {
                HideProgress();
            }
        }

        public void OnBossProgress(float percent)
        {
            progress.SetPercent(percent);
        }

        private void ShowProgress()
        {
            progressCanvasGroup.alpha = 1.0f;
        }

        private void HideProgress()
        {
            progressCanvasGroup.alpha = 0.0f;
        }
    }
}