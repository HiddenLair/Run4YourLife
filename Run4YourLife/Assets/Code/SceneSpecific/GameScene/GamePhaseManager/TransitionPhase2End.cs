using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Cinemachine;

namespace Run4YourLife.GameManagement
{
    public class TransitionPhase2End : TransitionBase
    {
        public override GamePhase GamePhase { get { return GamePhase.TransitionPhase2End; } }

        [SerializeField]
        private PlayableDirector m_startingCutscene;

        public override void StartPhase()
        {
            StartCoroutine(StartPhaseCoroutine());
        }

        private IEnumerator StartPhaseCoroutine()
        {         
            List<GameObject> runners = new List<GameObject>();
            runners.AddRange(GameplayPlayerManager.Instance.RunnersAlive);
            runners.AddRange(GameplayPlayerManager.Instance.GhostsAlive);
            foreach (GameObject runner in runners)
            {
                DeactivateScripts(runner);
            }
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            DeactivateScripts(boss);

            BindTimelineTracks(m_startingCutscene,runners, boss);
            m_startingCutscene.Play();
            yield return new WaitUntil(() => m_startingCutscene.state != PlayState.Playing); // wait until cutscene has completed
            Unbind(m_startingCutscene);
            foreach (GameObject runner in runners)
            {
                ActivateScripts(runner);
            }
            ActivateScripts(boss);
            GameManager.Instance.ChangeGamePhase(GamePhase.TransitionPhase3Start);
        }

        public override void EndPhase()
        {

        }

        public override void DebugStartPhase()
        {
            Debug.Log("This method should not be called");
        }

        public override void DebugEndPhase()
        {
            Debug.Log("This method should not be called");
        }
    }
}
