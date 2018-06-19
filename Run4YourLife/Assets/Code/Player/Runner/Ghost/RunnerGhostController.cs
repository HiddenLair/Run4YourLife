using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Run4YourLife.InputManagement;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Player {

    [RequireComponent(typeof(RunnerGhostControlScheme))]
    [RequireComponent(typeof(PlayerInstance))]
    public class RunnerGhostController : MonoBehaviour {

        [SerializeField]
        private float m_speed;

        [SerializeField]
        private Vector2 m_clampMin;

        [SerializeField]
        private Vector2 m_clampMax;

        [SerializeField]
        [HideInInspector]
        private RunnerGhostControlScheme m_controlScheme;

        [SerializeField]
        [HideInInspector]
        private PlayerInstance m_playerInstance;

        private Vector2 m_screenPosition;
        
        private void Reset()
        {
            m_controlScheme = GetComponent<RunnerGhostControlScheme>();
            m_playerInstance = GetComponent<PlayerInstance>(); 
        }

        private void OnEnable()
        {
            m_screenPosition = GetScreenPositionFromCurrentPosition();

            m_controlScheme.Active = true;
            //Todo Execute spawn playable
        }

        private Vector2 GetScreenPositionFromCurrentPosition()
        {
            Camera camera = CameraManager.Instance.MainCamera;
            Vector2 screenPosition = camera.WorldToScreenPoint(transform.position);
            screenPosition.x = screenPosition.x/camera.pixelWidth;
            screenPosition.y = screenPosition.y/camera.pixelHeight;
            return screenPosition;
        }

        private void OnDisable()
        {
            m_controlScheme.Active = false;
            //Todo Execute revive playable 
        }

        void Update()
        {
            MoveRunnerGhost();
        }

        private void MoveRunnerGhost()
        {
            m_screenPosition.x = Mathf.Clamp(m_screenPosition.x + m_speed * m_controlScheme.Horizontal.Value() * Time.deltaTime, m_clampMin.x, m_clampMax.x);
            m_screenPosition.y = Mathf.Clamp(m_screenPosition.y + m_speed * m_controlScheme.Vertical.Value()   * Time.deltaTime, m_clampMin.y, m_clampMax.y);

            Camera m_mainCamera = CameraManager.Instance.MainCamera;
            Vector3 screenSpacePosition = new Vector3()
            {
                x = m_screenPosition.x * m_mainCamera.pixelWidth,
                y = m_screenPosition.y * m_mainCamera.pixelHeight,
                z = Math.Abs(m_mainCamera.transform.position.z - transform.position.z)
            };

            transform.position = m_mainCamera.ScreenToWorldPoint(screenSpacePosition);
        }

        private void TrimPositionHorizontallyInsideCameraView()
        {
            Camera m_mainCamera = CameraManager.Instance.MainCamera;
            Vector3 bottomLeft = m_mainCamera.ScreenToWorldPoint(new Vector3(0, 0, Math.Abs(m_mainCamera.transform.position.z - transform.position.z)));
            Vector3 topRight = m_mainCamera.ScreenToWorldPoint(new Vector3(m_mainCamera.pixelWidth, m_mainCamera.pixelHeight, Math.Abs(m_mainCamera.transform.position.z - transform.position.z)));
            Vector3 trimmedPosition = transform.position;
            trimmedPosition.x = Mathf.Clamp(trimmedPosition.x, bottomLeft.x, topRight.x);
            trimmedPosition.y = Mathf.Clamp(trimmedPosition.y, bottomLeft.y, topRight.y);
            transform.position = trimmedPosition;
        }

        public void ReviveGhost()
        {
            GameplayPlayerManager.Instance.OnRunnerRevive(m_playerInstance.PlayerHandle, transform.position);
        }
    }
}
