using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Cinemachine;
using Run4YourLife.GameManagement.AudioManagement;
using System;

namespace Run4YourLife.GameManagement
{
    public class TransitionPhase1Start : TransitionBase
    {
        public override GamePhase GamePhase { get { return GamePhase.TransitionPhase1Start; } }

        [SerializeField]
        private PlayableDirector m_startingCutscene;

        [SerializeField]
        private CinemachineVirtualCamera m_cinemachineVirtualCamera;

        [SerializeField]
        private AudioClip phaseMusic;

        private PlayerSpawner m_playerSpawner;

        private Coroutine m_startPhaseCoroutine;      

        #region Initialization

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);
        }

        #endregion

        public override void StartPhase()
        {
            if(phaseMusic != null)
            {
                AudioManager.Instance.PlayMusic(phaseMusic);
            }

            m_startPhaseCoroutine = StartCoroutine(StartPhaseCoroutine());
        }

        private IEnumerator StartPhaseCoroutine()
        {
            CameraManager.Instance.TransitionToCamera(m_cinemachineVirtualCamera);
            
            StartCutscene();
            
            yield return new WaitUntil(() => m_startingCutscene.state != PlayState.Playing); // wait until cutscene has completed
            
            EndCutscene();

            GameManager.Instance.ChangeGamePhase(GamePhase.EasyMoveHorizontal);
        }

        private void StartCutscene()
        {
            m_playerSpawner.ActivateRunners();
            m_playerSpawner.ActivateBoss();

            foreach(GameObject runner in GameplayPlayerManager.Instance.Runners)
            {
                DeactivateScripts(runner);
            }

            DeactivateScripts(GameplayPlayerManager.Instance.Boss);

            BindTimelineTracks(m_startingCutscene,GameplayPlayerManager.Instance.Runners, GameplayPlayerManager.Instance.Boss);
            m_startingCutscene.Play();
        }

        private void EndCutscene()
        {
            Unbind(m_startingCutscene);
            foreach (GameObject runner in GameplayPlayerManager.Instance.Runners)
            {
                ActivateScripts(runner);
            }
            ActivateScripts(GameplayPlayerManager.Instance.Boss);
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
            StopCoroutine(m_startPhaseCoroutine);
                m_startPhaseCoroutine = null;

            EndCutscene();
        }
    }
}
