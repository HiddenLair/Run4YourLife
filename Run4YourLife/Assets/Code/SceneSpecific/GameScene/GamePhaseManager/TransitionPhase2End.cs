using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Run4YourLife.SceneSpecific;

namespace Run4YourLife.GameManagement
{
    public class TransitionPhase2End : TransitionBase
    {
        public override GamePhase GamePhase { get { return GamePhase.TransitionPhase2End; } }

        [SerializeField]
        private PlayableDirector m_portalCutscene;

        [SerializeField]
        private PlayableDirector m_endCutScene;

        [SerializeField]
        private GemColumnController[] m_columns;

        private Coroutine m_startPhaseCoroutine;

        public override void StartPhase()
        {
            m_startPhaseCoroutine = StartCoroutine(StartPhaseCoroutine());
        }

        private IEnumerator StartPhaseCoroutine()
        {
            List<GameObject> runners = new List<GameObject>();
            runners.AddRange(GameplayPlayerManager.Instance.RunnersAlive);
            runners.AddRange(GameplayPlayerManager.Instance.GhostsAlive);

            StartPortalCutScene();

            yield return new WaitUntil(() => m_portalCutscene.state != PlayState.Playing); // wait until cutscene has completed

            EndPortalCutScene();

            StartEndCutScene(runners);

            yield return new WaitUntil(() => m_endCutScene.state != PlayState.Playing); // wait until cutscene has completed

            EndCutScene(runners);

            GameManager.Instance.ChangeGamePhase(GamePhase.TransitionPhase3Start);
        }

        private void StartPortalCutScene()
        {
            m_portalCutscene.Play();
        }

        private void EndPortalCutScene()
        {
            m_portalCutscene.Stop();
        }

        private void StartEndCutScene(List<GameObject> runners)
        {
            GameplayPlayerManager.Instance.SetEventsToListen(false);

            foreach (GameObject runner in runners)
            {
                DeactivateScripts(runner);
            }
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            DeactivateScripts(boss);

            BindTimelineTracks(m_endCutScene, runners, boss);
            m_endCutScene.Play();
        }

        private void EndCutScene(List<GameObject> runners)
        {
            GameplayPlayerManager.Instance.SetEventsToListen(true);

            m_endCutScene.Stop();
            Unbind(m_endCutScene);
            foreach (GameObject runner in runners)
            {
                ActivateScripts(runner);
            }
            ActivateScripts(GameplayPlayerManager.Instance.Boss);
        }

        public override void EndPhase()
        {
            foreach(GemColumnController c in m_columns)
            {
                c.DeactivateColumn();
            }
        }

        public override void DebugStartPhase()
        {
            Debug.Log("This method should not be called");
        }

        public override void DebugEndPhase()
        {
            StopCoroutine(m_startPhaseCoroutine);
            m_startPhaseCoroutine = null;

            List<GameObject> runners = new List<GameObject>();
            runners.AddRange(GameplayPlayerManager.Instance.RunnersAlive);
            runners.AddRange(GameplayPlayerManager.Instance.GhostsAlive);
            EndPortalCutScene();
            EndCutScene(runners);

            GameplayPlayerManager.Instance.DebugClearPlayers();
        }
    }
}
