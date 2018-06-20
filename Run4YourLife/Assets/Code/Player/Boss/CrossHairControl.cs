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
        private enum State
        {
            Default,
            MovementLocked,
            PositionAndMovementLocked
        }

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
        private State m_state;

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
            UICrossHair.Instance.UpdatePosition(Position);
        }

        public void Move()
        {
            switch(m_state)
            {
                case State.Default:
                    float xInput = m_controlScheme.CrosshairHorizontal.Value();
                    float yInput = m_controlScheme.CrosshairVertical.Value();

                    m_screenPosition.x = Mathf.Clamp(m_screenPosition.x + m_speed * xInput * Time.deltaTime, m_clampMin.x, m_clampMax.x);
                    m_screenPosition.y = Mathf.Clamp(m_screenPosition.y + m_speed * yInput * Time.deltaTime, m_clampMin.y, m_clampMax.y);
                    break;
                case State.MovementLocked:
                    break;
                case State.PositionAndMovementLocked:
                    m_screenPosition = CameraManager.Instance.MainCamera.WorldToScreenPoint(m_lockedPosition);
                    break;
            }
        }

        public void LockMovement()
        {
            Debug.Assert(m_state == State.Default);
            m_state = State.MovementLocked;
        }

        public void UnlockMovement()
        {
            Debug.Assert(m_state == State.MovementLocked || m_state == State.Default);
            m_state = State.Default;
        }

        public void LockPositionAndMovement()
        {
            Debug.Assert(m_state == State.Default);
            m_state = State.PositionAndMovementLocked;
        }

        public void UnlockPositionAndMovement()
        {
            Debug.Assert(m_state == State.PositionAndMovementLocked || m_state == State.Default);
            m_state = State.Default;
        }
    }
}