using UnityEngine.EventSystems;
using UnityEngine;

using Run4YourLife.UI;

namespace Run4YourLife.Player {
    public class BossUnlock : MonoBehaviour {

        [SerializeField]
        private float bossPathPosition = 10;

        [SerializeField]
        private GameObject uiActions;

        private MonoBehaviour[] toUnlock;
        private BossPathWalker walker;

        private void Awake()
        {
            walker = GetComponent<BossPathWalker>();
            Debug.Assert(walker != null);

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
            uiActions.SetActive(true);
        }
    }
}
