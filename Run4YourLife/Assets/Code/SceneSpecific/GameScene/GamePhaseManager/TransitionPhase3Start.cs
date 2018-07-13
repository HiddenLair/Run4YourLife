using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

namespace Run4YourLife.GameManagement
{
    public class TransitionPhase3Start : TransitionBase
    {
        public override GamePhase GamePhase { get { return GamePhase.TransitionPhase3Start; } }

        [SerializeField]
        private PlayableDirector m_RunnersCutscene;

        [SerializeField]
        private PlayableDirector m_BossCutscene;

        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

        [SerializeField]
        private Transform portalTransform;

        [SerializeField]
        private Vector3 offsetFromPortal = new Vector3(0, -0.5f, 0);

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
            CameraManager.Instance.TransitionToCamera(m_virtualCamera);

            StartCutSceneRunners();

            yield return new WaitUntil(() => m_RunnersCutscene.state != PlayState.Playing); // wait until cutscene has completed

            EndCutSceneRunners();
            StartCutSceneBoss();
            
            yield return new WaitUntil(() => m_BossCutscene.state != PlayState.Playing); // wait until cutscene has completed

            EndCutSceneBoss();

            GameManager.Instance.ChangeGamePhase(GamePhase.HardMoveHorizontal);
        }

        private void StartCutSceneRunners()
        {
            List<GameObject> runners = GameplayPlayerManager.Instance.RunnersAlive;
            foreach (GameObject runner in runners)
            {
                runner.transform.position = portalTransform.position + offsetFromPortal;
                runner.SetActive(false);
                DeactivateScripts(runner);
            }

            //Runners intro
            BindTimelineTracks(m_RunnersCutscene, runners, null);
            m_RunnersCutscene.Play();
        }

        private void EndCutSceneRunners()
        {
            Unbind(m_RunnersCutscene);
            foreach (GameObject runner in GameplayPlayerManager.Instance.RunnersAlive)
            {
                ActivateScripts(runner);
            }
        }

        private void StartCutSceneBoss()
        {
            //Boss intro
            GameObject boss = m_playerSpawner.ActivateBoss();
            DeactivateScripts(boss);
            BindTimelineTracks(m_BossCutscene, null, boss);
            m_BossCutscene.Play();
        }

        private void EndCutSceneBoss()
        {
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

            EndCutSceneRunners();
            EndCutSceneBoss();

            GameplayPlayerManager.Instance.DebugClearPlayers();
        }

        public override void DebugStartPhase()
        {
            m_playerSpawner.ActivateRunners();
            m_startPhaseCoroutine = StartCoroutine(StartPhaseCoroutine());
        }
    }
}

