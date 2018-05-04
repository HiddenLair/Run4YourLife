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

        void Update()
        {
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            if (boss != null)
            {
                IProgressProvider progressProvider = boss.GetComponent<IProgressProvider>();
                ExecuteEvents.Execute<IUIEvents>(m_uiManager, null, (x, y) => x.OnBossProgress(progressProvider.Progress));
            }
        }
    }
}