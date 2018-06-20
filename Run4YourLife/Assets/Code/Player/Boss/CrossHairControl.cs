using System;
using Run4YourLife.GameManagement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Run4YourLife.Player
{
    public class CrossHairControl : MonoBehaviour
    {
        [SerializeField]
        private float m_speed;

        [SerializeField]
        private Vector2 m_clampMin;

        [SerializeField]
        private Vector2 m_clampMax;



        private bool m_isLocked;
        private bool m_totalLocked;

        // Normalized position range from (0,0) to (1,1)
        private Vector2 m_screenPosition;

        private Vector3 lastPos;

        public Vector3 Position
        {
            get
            {
                Camera m_mainCamera = CameraManager.Instance.MainCamera;
                Vector3 screenSpacePosition = new Vector3()
                {
                    x = m_screenPosition.x * m_mainCamera.pixelWidth,
                    y = m_screenPosition.y * m_mainCamera.pixelHeight,
                    z = Math.Abs(m_mainCamera.transform.position.z)
                };

                return m_mainCamera.ScreenToWorldPoint(screenSpacePosition);
            }
        }

        public void Move(Vector2 input)
        {
            if(!m_isLocked)
            {
                m_screenPosition.x = Mathf.Clamp(m_screenPosition.x + m_speed * input.x * Time.deltaTime, m_clampMin.x, m_clampMax.x);
                m_screenPosition.y = Mathf.Clamp(m_screenPosition.y + m_speed * input.y * Time.deltaTime, m_clampMin.y, m_clampMax.y);

                UICrossHair.Instance.UpdatePosition(Position);
            }
        }

        /*void Update()
        {
            if (m_totalLocked)
            {
                m_crossHairGameObject.transform.position = lastPos;
                OverrideZ();
            } 
        }*/

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
            //lastPos = m_crossHairGameObject.transform.position;
        }

        public void TotalUnlock()
        {
            Unlock();
            m_totalLocked = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(Position, 0.5f);
            Gizmos.DrawLine(transform.position, Position);
            Debug.Log(Position);
        }
    }
}