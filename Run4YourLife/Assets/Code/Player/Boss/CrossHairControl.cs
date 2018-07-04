using System;

using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.GameManagement;
using Run4YourLife.InputManagement;
using Run4YourLife.CameraUtils;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(BossControlScheme))]
    public class CrossHairControl : MonoBehaviour
    {
        [SerializeField]
        private float m_speed;

        [SerializeField]
        private Vector2 m_clampMin;

        [SerializeField]
        private Vector2 m_clampMax;

        // Normalized position range from (0,0) to (1,1)
        private Vector2 m_screenPosition;

        private BossControlScheme m_controlScheme;

        private void Awake()
        {
            m_controlScheme = GetComponent<BossControlScheme>();
        }

        public Vector3 Position
        {
            get
            {
                Camera mainCamera = CameraManager.Instance.MainCamera;
                /*Vector3 screenSpacePosition = new Vector3()
                {
                    x = m_screenPosition.x * m_mainCamera.pixelWidth,
                    y = m_screenPosition.y * m_mainCamera.pixelHeight,
                    z = Math.Abs(m_mainCamera.transform.position.z)
                };

                return m_mainCamera.ScreenToWorldPoint(screenSpacePosition);*/

                Vector3 normalizedViewport = new Vector3()
                {
                    x = m_screenPosition.x,
                    y = m_screenPosition.y,
                    z = Math.Abs(mainCamera.transform.position.z)
                };
                return CameraConverter.NormalizedViewportToWorldPosition(mainCamera, normalizedViewport);
            }
        }

        private void Update()
        {
            Move();
            UICrossHair.Instance.UpdatePosition(Position);
        }

        public void Move()
        {
            float xInput = m_controlScheme.CrosshairHorizontal.Value();
            float yInput = m_controlScheme.CrosshairVertical.Value();

            m_screenPosition.x = Mathf.Clamp(m_screenPosition.x + m_speed * xInput * Time.deltaTime, m_clampMin.x, m_clampMax.x);
            m_screenPosition.y = Mathf.Clamp(m_screenPosition.y + m_speed * yInput * Time.deltaTime, m_clampMin.y, m_clampMax.y);
        }
    }
}