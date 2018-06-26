using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Cinemachine;

namespace Run4YourLife.GameManagement
{
    public class TransitionToBossFightPhaseManager : GamePhaseManager
    {
        public override GamePhase GamePhase { get { return GamePhase.TransitionToBossFight; } }

        [SerializeField]
        private PlayableDirector m_startingCutscene;

        private TimelineAsset timelineAsset;

        public override void StartPhase()
        {
            StartCoroutine(StartPhaseCoroutine());
        }

        private IEnumerator StartPhaseCoroutine()
        {

            List<GameObject> runners = GameplayPlayerManager.Instance.Runners;
            foreach (GameObject runner in runners)
            {
                DeactivateScripts(runner);
            }
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            DeactivateScripts(boss);

            BindTimelineTracks(runners, boss);
            m_startingCutscene.RebuildGraph();
            m_startingCutscene.Play();
            yield return new WaitUntil(() => m_startingCutscene.state != PlayState.Playing); // wait until cutscene has completed
            Unbind();
            foreach (GameObject runner in runners)
            {
                ActivateScripts(runner);
            }
            ActivateScripts(boss);
            GameManager.Instance.ChangeGamePhase(GamePhase.BossFight);
        }

        private void DeactivateScripts(GameObject g)
        {
            foreach (MonoBehaviour mono in g.GetComponents<MonoBehaviour>())
            {
                mono.enabled = false;
            }
        }

        private void ActivateScripts(GameObject g)
        {
            foreach (MonoBehaviour mono in g.GetComponents<MonoBehaviour>())
            {
                mono.enabled = true;
            }
            Animator anim = g.GetComponent<Animator>();
            Avatar temp = anim.avatar;
            anim.avatar = null;
            anim.avatar = temp;
        }

        private void BindTimelineTracks(List<GameObject> runners, GameObject boss)
        {
            timelineAsset = (TimelineAsset)m_startingCutscene.playableAsset;
            var outputs = timelineAsset.outputs;
            foreach (var itm in outputs)
            {              
                if (itm.streamName.Contains("Player1") && runners.Count > 0)
                {
                    m_startingCutscene.SetGenericBinding(itm.sourceObject, runners[0]);
                }
                else if (itm.streamName.Contains("Player2") && runners.Count > 1)
                {
                    m_startingCutscene.SetGenericBinding(itm.sourceObject, runners[1]);
                }
                else if (itm.streamName.Contains("Player3") && runners.Count > 2)
                {
                    m_startingCutscene.SetGenericBinding(itm.sourceObject, runners[2]);
                }
                else if (itm.streamName.Contains("Boss"))
                {
                    m_startingCutscene.SetGenericBinding(itm.sourceObject, boss);
                }
            }
        }

        private void Unbind()
        {
            timelineAsset = (TimelineAsset)m_startingCutscene.playableAsset;
            var outputs = timelineAsset.outputs;
            foreach (var itm in outputs)
            {
                m_startingCutscene.SetGenericBinding(itm.sourceObject, null);
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
            
        }
    }
}
