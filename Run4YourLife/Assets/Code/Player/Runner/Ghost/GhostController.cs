using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Run4YourLife.InputManagement;

namespace Run4YourLife.Player {
    public class GhostController : MonoBehaviour {
        #region Inspector
        [SerializeField]
        private float speed;
        #endregion
        #region Variables
        private RunnerControlScheme m_runnerControlScheme;
        #endregion
        void Awake()
        {
            m_runnerControlScheme = GetComponent<RunnerControlScheme>();          
        }

        private void OnEnable()
        {
            m_runnerControlScheme.Active = true;
        }

        private void OnDisable()
        {
            m_runnerControlScheme.Active = false;
        }

        // Update is called once per frame
        void Update() {
            float horizontal = m_runnerControlScheme.Move.Value();
            float vertical = m_runnerControlScheme.Vertical.Value();
            transform.Translate(new Vector3(horizontal * speed * Time.deltaTime,vertical * speed * Time.deltaTime,0));
            TrimPlayerPositionHorizontalInsideCameraView();
        }

        private void TrimPlayerPositionHorizontalInsideCameraView()
        {
            Camera m_mainCamera = Camera.main;
            Vector3 bottomLeft = m_mainCamera.ScreenToWorldPoint(new Vector3(0, 0, Math.Abs(m_mainCamera.transform.position.z - transform.position.z)));
            Vector3 topRight = m_mainCamera.ScreenToWorldPoint(new Vector3(m_mainCamera.pixelWidth, m_mainCamera.pixelHeight, Math.Abs(m_mainCamera.transform.position.z - transform.position.z)));
            Vector3 trimmedPosition = transform.position;
            trimmedPosition.x = Mathf.Clamp(trimmedPosition.x, bottomLeft.x, topRight.x);
            trimmedPosition.y = Mathf.Clamp(trimmedPosition.y, bottomLeft.y, topRight.y);
            transform.position = trimmedPosition;
        }
    }
}
