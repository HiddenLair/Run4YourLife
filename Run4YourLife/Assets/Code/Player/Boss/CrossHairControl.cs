using System;
using Run4YourLife.GameManagement;
using Run4YourLife.InputManagement;
using UnityEngine;
using UnityEngine.EventSystems;

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
        // World position the crosshair keeps when position is locked
        private Vector3 m_lockedPosition;
        private bool m_isMovementLocked;
        private bool m_isPositionLocked;


        private BossControlScheme m_controlScheme;

        private void Awake()
        {
            m_controlScheme = GetComponent<BossControlScheme>();
        }

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

        private void Update()
        {
            Move();
            UpdateUI();
        }

        private void UpdateUI()
        {
            if(m_isPositionLocked)
            {
                UICrossHair.Instance.UpdatePosition(m_lockedPosition);
            }
            else
            {
                UICrossHair.Instance.UpdatePosition(Position);
            }
        }

        public void Move()
        {
            if(!m_isMovementLocked && !m_isPositionLocked)
            {
                float xInput = m_controlScheme.CrosshairHorizontal.Value();
                float yInput = m_controlScheme.CrosshairVertical.Value();

                m_screenPosition.x = Mathf.Clamp(m_screenPosition.x + m_speed * xInput * Time.deltaTime, m_clampMin.x, m_clampMax.x);
                m_screenPosition.y = Mathf.Clamp(m_screenPosition.y + m_speed * yInput * Time.deltaTime, m_clampMin.y, m_clampMax.y);
            }
        }

        public void LockMovement()
        {
            m_isMovementLocked = true;
        }

        public void UnlockMovement()
        {
            m_isMovementLocked = false;
        }

        public void LockPosition()
        {
            m_isPositionLocked = true;
            m_lockedPosition = Position;
        }

        public void UnlockPosition()
        {
            
            m_isPositionLocked = false;
            m_screenPosition = CameraManager.Instance.MainCamera.WorldToViewportPoint(m_lockedPosition);
        }

        public void LockPositionAndMovement()
        {
            LockMovement();
            LockPosition();
        }

        public void UnlockPositionAndMovement()
        {
            UnlockMovement();
            UnlockPosition();
        }
    }
}