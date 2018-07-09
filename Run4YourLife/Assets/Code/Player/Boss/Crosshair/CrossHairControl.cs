using System;

using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.UI;
using Run4YourLife.CameraUtils;
using Run4YourLife.GameManagement;
using Run4YourLife.InputManagement;

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
        [SerializeField]
        private Vector2 m_screenPosition;

        private BossControlScheme m_controlScheme;

        private void Awake()
        {
            m_controlScheme = GetComponent<BossControlScheme>();

            UICrossHair.Instance.UpdatePosition(Position);
        }

        public Vector3 Position
        {
            get
            {
                return CameraConverter.ViewportToGamePlaneWorldPosition(CameraManager.Instance.MainCamera, m_screenPosition);
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