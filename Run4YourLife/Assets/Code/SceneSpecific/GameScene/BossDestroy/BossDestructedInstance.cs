using UnityEngine;

namespace Run4YourLife.GameManagement
{
    public abstract class BossDestructedInstance : MonoBehaviour
    {

        [SerializeField]
        private float m_distance;

        [SerializeField]
        private bool m_isDynamic;

        public float DestroyPosition { get { return transform.position.x + m_distance; } }

        public bool IsDestructed { get; protected set; }

        private void Awake()
        {
            if(m_isDynamic)
            {
                BossDestructorManager.Instance.AddDynamic(this);
            }
        }

        protected virtual void Start()
        {
            if(!m_isDynamic)
            {
                BossDestructorManager.Instance.AddStatic(this);
            }
        }

        public abstract void OnBossDestroy();

        public abstract void OnRegenerate();

        protected virtual void OnDrawGizmosSelected()
        {
            Vector3 position = transform.position;
            position.x = DestroyPosition;
            Gizmos.DrawSphere(position, 0.5f);
        }
    }
}