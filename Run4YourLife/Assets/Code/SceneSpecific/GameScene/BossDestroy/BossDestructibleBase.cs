using UnityEngine;

namespace Run4YourLife.GameManagement
{
    public abstract class BossDestructibleBase : MonoBehaviour, IBossDestructible
    {

        [SerializeField]
        private float m_distance;

        [SerializeField]
        private bool m_isStatic = true;

        [SerializeField]
        private bool m_canBeRegenerated = true;

        private BossDestructibleState m_bossDestructibleState;

        private IBossDestructibleNotified[] m_bossDestructiblesNotified;

        public float DestroyPosition { get { return transform.position.x + m_distance; } }

        public BossDestructibleState BossDestructibleState { get { return m_bossDestructibleState; } }

        public bool IsRegeneratable
        {
            get
            {
                return transform.parent.gameObject.activeInHierarchy;
            }
        }

        protected virtual void OnDisable()
        {
            m_bossDestructibleState = BossDestructibleState.Destroyed;
        }

        protected virtual void OnEnable()
        {
            m_bossDestructibleState = BossDestructibleState.Alive;
        }

        protected virtual void Awake()
        {
            m_bossDestructiblesNotified = GetComponentsInChildren<IBossDestructibleNotified>();

            if (m_isStatic)
            {
                BossDestructionManager.Instance.AddStatic(this);
            }
            else
            {
                BossDestructionManager.Instance.AddDynamic(this);
            }
        }

        public void Destroy()
        {
            m_bossDestructibleState = BossDestructibleState.InDestruction;

            foreach (IBossDestructibleNotified bossDestructibleNotified in m_bossDestructiblesNotified)
            {
                bossDestructibleNotified.OnDestroyed();
            }

            DestroyBehaviour();
        }

        protected abstract void DestroyBehaviour();

        public void Regenerate()
        {
            if (m_canBeRegenerated)
            {
                m_bossDestructibleState = BossDestructibleState.Alive;

                RegenerateBehaviour();

                foreach (IBossDestructibleNotified bossDestructibleNotified in m_bossDestructiblesNotified)
                {
                    bossDestructibleNotified.OnRegenerated();
                }
            }
        }

        protected abstract void RegenerateBehaviour();

        protected virtual void OnDrawGizmosSelected()
        {
            Vector3 position = transform.position;
            position.x = DestroyPosition;
            Gizmos.DrawSphere(position, 0.5f);
        }
    }
}