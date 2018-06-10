using UnityEngine;
using UnityEngine.EventSystems;

namespace Run4YourLife.Player
{
    public class CrossHairControl : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_crossHairGameObject;

        [SerializeField]
        private float m_speed;

        [SerializeField]
        private float desiredZ = 0.0f;

        private Transform m_crossHairTransform;
        private GameObject m_ui;

        private bool m_isLocked;
        private bool m_totalLocked;
        private Vector3 lastPos;

        private void Awake()
        {
            m_crossHairTransform = m_crossHairGameObject.transform;
            
            m_ui = GameObject.FindGameObjectWithTag(Tags.UI);
            Debug.Assert(m_ui != null);
        }

        private void OnEnable()
        {
            ExecuteEvents.Execute<IUICrossHairEvents>(m_ui, null, (a,b) => a.AttachCrossHair(m_crossHairTransform));
        }

        private void OnDisable()
        {
            ExecuteEvents.Execute<IUICrossHairEvents>(m_ui, null, (a,b) => a.DeatachCrossHair());
        }

        public Vector3 Position
        {
            get
            {
                return m_crossHairTransform.position;
            }
        }

        public void Translate(Vector3 input)
        {
            if(!m_isLocked)
            {
                m_crossHairTransform.Translate(input * m_speed * Time.deltaTime);
                OverrideZ();
            }
        }

        public void ChangePosition(Vector3 newPosition)
        {
            if(!m_isLocked)
            {
                m_crossHairGameObject.transform.position = newPosition;
                OverrideZ();
            }

        }

        void Update()
        {
            if (m_totalLocked)
            {
                m_crossHairGameObject.transform.position = lastPos;
                OverrideZ();
            } 
        }

        public void Lock()
        {
            m_isLocked = true;
        }

        public void Unlock()
        {
            m_isLocked = false;
        }

        public void TotalLock()
        {
            Lock();
            m_totalLocked = true;
            lastPos = m_crossHairGameObject.transform.position;
        }

        public void TotalUnlock()
        {
            Unlock();
            m_totalLocked = false;
        }

        private void OverrideZ()
        {
            Vector3 tmpPosition = m_crossHairTransform.localPosition; tmpPosition.z = desiredZ;
            m_crossHairTransform.localPosition = tmpPosition;
        }
    }
}