using UnityEngine.EventSystems;
using UnityEngine;

namespace Run4YourLife.Player {
    public class BossUnlock : MonoBehaviour {

        [SerializeField]
        private float bossPathPosition = 10;

        [SerializeField]
        private GameObject uiActions;

        private MonoBehaviour[] toUnlock;
        private BossPathWalker walker;
        private GameObject m_ui;

        private void Awake()
        {
            walker = GetComponent<BossPathWalker>();
            Debug.Assert(walker != null);

            m_ui = GameObject.FindGameObjectWithTag(Tags.UI);
            Debug.Assert(m_ui != null);

            toUnlock = gameObject.GetComponents<MonoBehaviour>();
        }

        private void Update()
        {
           if(walker.m_position >= bossPathPosition)
            {
                Unlock();
            }
        }

        private void Unlock()
        {
            foreach (MonoBehaviour m in toUnlock)
            {
                m.enabled = true;
            }
            ExecuteEvents.Execute<IUICrossHairEvents>(m_ui, null, (a, b) => a.ShowCrossHair());
            uiActions.SetActive(true);
        }
    }
}
