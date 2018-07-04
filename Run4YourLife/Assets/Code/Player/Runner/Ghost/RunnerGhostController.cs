using System;

using UnityEngine;

using Run4YourLife.InputManagement;
using Run4YourLife.GameManagement;
using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.CameraUtils;

namespace Run4YourLife.Player {

    [RequireComponent(typeof(RunnerGhostControlScheme))]
    [RequireComponent(typeof(PlayerInstance))]
    public class RunnerGhostController : MonoBehaviour {

        [SerializeField]
        private float m_speed;

        [SerializeField]
        [Tooltip("Offset that will be added to ghost's transform. Where the runner will be revived at")]
        private Vector3 m_reviveRunnerOffset;

        [SerializeField]
        private AudioClip m_revivePlayerSound;

        [SerializeField]
        private RunnerGhostControlScheme m_controlScheme;

        [SerializeField]
        private PlayerInstance m_playerInstance;
        
        [SerializeField]
        private FXReceiver m_reviveParticles;

        [SerializeField]
        private Transform m_graphics;

        private bool m_isFacingRight = true;
        
        private void Reset()
        {
            m_controlScheme = GetComponent<RunnerGhostControlScheme>();
            m_playerInstance = GetComponent<PlayerInstance>();
            m_reviveParticles = transform.Find("ReviveParticles").GetComponent<FXReceiver>();
            m_graphics = transform.Find("Graphics");
        }

        private void OnEnable()
        {
            m_controlScheme.Active = true;
            //Todo Execute ghost spawn playable
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
            Vector3 position = transform.position;
            
            position.x = position.x + m_speed * m_controlScheme.Horizontal.Value() * Time.deltaTime;
            position.y = position.y + m_speed * m_controlScheme.Vertical.Value()   * Time.deltaTime;
            
            LookAtMovingSide();
            TrimPlayerPositionInsideCameraView(ref position);
            
            transform.position = position;
        }

        private void TrimPlayerPositionInsideCameraView(ref Vector3 position)
        {
            Camera mainCamera = CameraManager.Instance.MainCamera;
            
            Vector3 screenBottomLeft = CameraConverter.NormalizedViewportToGamePlaneWorldPosition(mainCamera, new Vector2(0,0));
            Vector3 screenTopRight = CameraConverter.NormalizedViewportToGamePlaneWorldPosition(mainCamera, new Vector2(1,1));

            position.x = Mathf.Clamp(position.x, screenBottomLeft.x, screenTopRight.x);
            position.y = Mathf.Clamp(position.y, screenBottomLeft.y, screenTopRight.y);
        }

        private void LookAtMovingSide()
        {
            bool shouldFaceTheOtherWay = (m_isFacingRight && m_controlScheme.Horizontal.Value() < 0) || (!m_isFacingRight && m_controlScheme.Horizontal.Value() > 0);
            if (shouldFaceTheOtherWay)
            {
                m_graphics.Rotate(Vector3.up, 180);
                m_isFacingRight = !m_isFacingRight;
            }
        }

        private void ReviveRunner()
        {
            AudioManager.Instance.PlaySFX(m_revivePlayerSound);
            m_reviveParticles.PlayFx(false);

            GameplayPlayerManager.Instance.OnRunnerRevive(m_playerInstance.PlayerHandle, transform.position + m_reviveRunnerOffset);
        }

        public void OnOtherRunnerCollidedGhost()
        {
            ReviveRunner();
        }
    }
}
