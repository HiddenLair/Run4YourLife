using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.Player;
using Cinemachine;

namespace Run4YourLife.GameManagement
{
    public class StartTutorial : GamePhaseManager
    {
        public override GamePhase GamePhase { get { return GamePhase.StartTutorial; } }

        [SerializeField]
        private AudioClip m_phase1MusicClip;

        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

        private PlayerSpawner m_playerSpawner;

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);
        }

        public override void StartPhase()
        {

            m_playerSpawner.ActivateRunners();

            AudioManager.Instance.PlayMusic(m_phase1MusicClip);

            GameObject boss = GameplayPlayerManager.Instance.Boss;
            Debug.Assert(boss != null);

            m_virtualCamera.Follow = null;
            m_virtualCamera.LookAt = null;
            CameraManager.Instance.TransitionToCamera(m_virtualCamera);

            foreach (GameObject runner in GameplayPlayerManager.Instance.Runners)
            {
                RunnerController runnerCharacterController = runner.GetComponent<RunnerController>();
                runnerCharacterController.CheckOutScreen = true;
            }
        }

        public override void EndPhase()
        {
            throw new System.NotImplementedException();
        }

        public override void DebugStartPhase()
        {
            Debug.LogError("This method should never be called");
        }

        public override void DebugEndPhase()
        {
            Debug.LogError("This method should never be called");
        }
    }
}