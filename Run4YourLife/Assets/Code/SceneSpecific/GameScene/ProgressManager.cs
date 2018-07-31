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
        private IUIProgressEvents m_progressEventUI;

        void Awake()
        {
            GameObject ui = GameObject.FindGameObjectWithTag(Tags.UI);
            Debug.Assert(ui != null);
            m_progressEventUI = ui.GetComponent<IUIProgressEvents>();
            Debug.Assert(m_progressEventUI != null);
        }

        void Update()
        {
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            if (boss != null)
            {
                IProgressProvider progressProvider = boss.GetComponent<IProgressProvider>();
                if (progressProvider != null)
                {
                    m_progressEventUI.OnBossProgress(progressProvider.Progress);
                }
            }
        }
    }
}