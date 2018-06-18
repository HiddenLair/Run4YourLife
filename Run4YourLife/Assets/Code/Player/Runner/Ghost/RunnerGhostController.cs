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
        [HideInInspector]
        private RunnerGhostControlScheme m_controlScheme;

        [SerializeField]
        [HideInInspector]
        private PlayerInstance m_playerInstance;
        
        private void Reset()
        {
            m_controlScheme = GetComponent<RunnerGhostControlScheme>();
            m_playerInstance = GetComponent<PlayerInstance>(); 
        }

        private void OnEnable()
        {
            m_controlScheme.Active = true;
            //Todo Execute spawn playable
        }

        private void OnDisable()
        {
            m_controlScheme.Active = false;
            //Todo Execute revive playable 
        }

        void Update()
        {
            MoveRunnerGhost();
            TrimPositionHorizontallyInsideCameraView();
        }

        private void MoveRunnerGhost()
        {
            Vector3 velocity = new Vector3()
            {
                x = m_speed * m_controlScheme.Horizontal.Value(),
                y = m_speed * m_controlScheme.Vertical.Value()
            };

            transform.Translate(velocity * Time.deltaTime);
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
