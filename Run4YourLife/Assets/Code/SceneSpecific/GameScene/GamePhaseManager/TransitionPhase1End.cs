using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

namespace Run4YourLife.GameManagement
{
    public class TransitionPhase1End : TransitionBase
    {
        public override GamePhase GamePhase { get { return GamePhase.TransitionPhase1End; } }

        [SerializeField]
        private PlayableDirector m_startingCutscene;

        [SerializeField]
        private CinemachineVirtualCamera m_virtualCamera;

        private Coroutine m_startPhaseCoroutine;

        public override void StartPhase()
        {
            m_startPhaseCoroutine = StartCoroutine(StartPhaseCoroutine());
        }

        private IEnumerator StartPhaseCoroutine()
        {
            CameraManager.Instance.TransitionToCamera(m_virtualCamera);

            List<GameObject> runners = new List<GameObject>();
            runners.AddRange(GameplayPlayerManager.Instance.RunnersAlive);
            runners.AddRange(GameplayPlayerManager.Instance.GhostsAlive);

            StartCutScene(runners);

            yield return new WaitUntil(() => m_startingCutscene.state != PlayState.Playing); // wait until cutscene has completed

            EndCutScene(runners);

            GameManager.Instance.ChangeGamePhase(GamePhase.TransitionPhase2Start);
        }

        private void StartCutScene(List<GameObject>runners)
        {
            GameplayPlayerManager.Instance.SetEventsToListen(false);

            foreach (GameObject runner in runners)
            {
                DeactivateScripts(runner);
            }
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            DeactivateScripts(boss);

            BindTimelineTracks(m_startingCutscene, runners, boss);
            m_startingCutscene.Play();
        }

        private void EndCutScene(List<GameObject> runners)
        {
            GameplayPlayerManager.Instance.SetEventsToListen(true);
            m_startingCutscene.Stop();
            Unbind(m_startingCutscene);
            foreach (GameObject runner in runners)
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

            List<GameObject> runners = new List<GameObject>();
            runners.AddRange(GameplayPlayerManager.Instance.RunnersAlive);
            runners.AddRange(GameplayPlayerManager.Instance.GhostsAlive);
            EndCutScene(runners);

            GameplayPlayerManager.Instance.DebugClearPlayers();
        }
    }
}
