﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Playables;

using Cinemachine;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.GameManagement
{
    public class TransitionPhase2Start : TransitionBase
    {
        public override GamePhase GamePhase { get { return GamePhase.TransitionPhase2Start; } }

        [SerializeField]
        private PlayableDirector m_RunnersCutscene;

        [SerializeField]
        private PlayableDirector m_PortalCutScene;

        [SerializeField]
        private PlayableDirector m_BossCutscene;

        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

        [SerializeField]
        private Transform portalTransform;

        [SerializeField]
        private Vector3 offsetFromPortal = new Vector3(0, -0.5f, 0);

        [SerializeField]
        private AudioClip m_phaseMusic;

        [SerializeField]
        private float m_musicFadeOutDuration;

        private Coroutine m_startPhaseCoroutine;
        private PlayerSpawner m_playerSpawner;

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);
        }

        public override void StartPhase()
        {
            GameplayPlayerManager.Instance.ReviveAllRunners();
            m_startPhaseCoroutine = StartCoroutine(StartPhaseCoroutine());
        }

        private IEnumerator StartPhaseCoroutine()
        {
            AudioManager.Instance.PlayMusicAfterFadeOut(m_phaseMusic, m_musicFadeOutDuration);

            CameraManager.Instance.TransitionToCamera(m_virtualCamera);

            StartRunnersCutScene();
            StartPortalCutScene();
            StartBossCutScene();

            yield return new WaitUntil(() => m_RunnersCutscene.state != PlayState.Playing); // wait until cutscene has completed

            EndRunnersCutScene();

            yield return new WaitUntil(() => m_BossCutscene.state != PlayState.Playing); // wait until cutscene has completed

            EndPortalCutScene();
            EndBossCutScene();
            

            GameManager.Instance.ChangeGamePhase(GamePhase.BossFight);
        }

        private void StartRunnersCutScene()
        {
            List<GameObject> runners = GameplayPlayerManager.Instance.RunnersAlive;
            foreach (GameObject runner in runners)
            {
                runner.transform.position = portalTransform.position + offsetFromPortal;
                runner.SetActive(false);
                DeactivateScripts(runner);
            }
            GameObject boss = m_playerSpawner.ActivateBoss();
            DeactivateScripts(boss);

            //Runners intro
            BindTimelineTracks(m_RunnersCutscene, runners, boss);
            m_RunnersCutscene.Play();
        }

        private void EndRunnersCutScene()
        {
            m_RunnersCutscene.Stop();
            Unbind(m_RunnersCutscene);
            foreach (GameObject runner in GameplayPlayerManager.Instance.RunnersAlive)
            {
                ActivateScripts(runner);
            }
        }

        private void StartPortalCutScene()
        {
            BindAudio(m_PortalCutScene);
            m_PortalCutScene.Play();
        }

        private void EndPortalCutScene()
        {
            Unbind(m_PortalCutScene);
            m_PortalCutScene.Stop();
        }

        private void StartBossCutScene()
        {
            //Boss intro
            BindTimelineTracks(m_BossCutscene, GameplayPlayerManager.Instance.RunnersAlive, GameplayPlayerManager.Instance.Boss);
            m_BossCutscene.Play();
        }

        private void EndBossCutScene()
        {
            m_BossCutscene.Stop();
            Unbind(m_BossCutscene);
            ActivateScripts(GameplayPlayerManager.Instance.Boss);
        }

        public override void EndPhase()
        {

        }

        public override void DebugEndPhase()
        {
            StopCoroutine(m_startPhaseCoroutine);
            m_startPhaseCoroutine = null;

            EndRunnersCutScene();
            EndPortalCutScene();
            EndBossCutScene();

            GameplayPlayerManager.Instance.DebugClearPlayers();
        }

        public override void DebugStartPhase()
        {
            m_playerSpawner.ActivateRunners();
            m_startPhaseCoroutine = StartCoroutine(StartPhaseCoroutine());
        }
    }
}
