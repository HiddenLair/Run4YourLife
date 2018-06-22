﻿using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.Player;

namespace Run4YourLife.GameManagement
{
    public class StartTutorial : GamePhaseManager
    {
        public override GamePhase GamePhase { get { return GamePhase.StartTutorial; } }

        [SerializeField]
        private AudioClip m_phase1MusicClip;

        private PlayerSpawner m_playerSpawner;

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);
        }

        public override void StartPhase()
        {
            List<GameObject> runners = m_playerSpawner.ActivateRunners();

            AudioManager.Instance.PlayMusic(m_phase1MusicClip);

            Camera cam = CameraManager.Instance.MainCamera;
            List<Transform> runnersTransform = new List<Transform>();
            foreach(GameObject o in runners)
            {
                runnersTransform.Add(o.transform);
            }
            SharedCamera script = cam.GetComponent<SharedCamera>();
            script.SetTargets(runnersTransform.ToArray());
            script.enabled = true;

            foreach (GameObject runner in GameplayPlayerManager.Instance.Runners)
            {
                RunnerController runnerCharacterController = runner.GetComponent<RunnerController>();
                runnerCharacterController.CheckOutScreen = true;
            }
        }

        public override void EndPhase()
        {

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