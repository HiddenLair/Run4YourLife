using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Run4YourLife.Player;
using System;

namespace Run4YourLife.GameManagement
{
    public class BossWinTransition : GamePhaseManager
    {
        public override GamePhase GamePhase { get { return GamePhase.BossWin; } }

        [SerializeField]
        private PlayableDirector m_startingCutscene;

        private TimelineAsset timelineAsset;

        public override void StartPhase()
        {
            StartCoroutine(StartPhaseCoroutine());
        }

        private IEnumerator StartPhaseCoroutine()
        {
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            List<GameObject> ghosts = GameplayPlayerManager.Instance.Ghosts;

            foreach (GameObject ghost in ghosts)
            {
                DeactivateScripts(ghost);
            }

            DeactivateScripts(boss);

            BindTimelineTracks(ghosts, boss);
            BindEndLocationTransformReference(boss.GetComponent<AbsorbPartGetter>().GetPartToAbsorb(),"Move",0,0);
            m_startingCutscene.Play();

            yield return new WaitUntil(() => m_startingCutscene.state != PlayState.Playing); // wait until cutscene has completed

            UnbindTimelineTracks();

            GameManager.Instance.EndGame_BossWin();
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

        private void BindEndLocationTransformReference(Transform t,String trackStringId, int minClipNum , int maxClipNum)
        {
            TimelineAsset tl = m_startingCutscene.playableAsset as TimelineAsset;

            //find the control track;
            var tracks = tl.GetOutputTracks();

            foreach(TrackAsset track in tracks)
            {
                if (track.name.Contains(trackStringId)) {
                    //create the new clip on the TL
                    var tlClips = track.GetClips();
                    int i = 0;
                    foreach (TimelineClip tlClip in tlClips)
                    {
                        TransformTweenClip clip = tlClip.underlyingAsset as TransformTweenClip;
                        if(i >= minClipNum && i <= maxClipNum)
                        {
                            clip.endLocation.exposedName = UnityEditor.GUID.Generate().ToString();
                            m_startingCutscene.SetReferenceValue(clip.endLocation.exposedName, t);
                        }
                        ++i;
                    }
                }
            }
        }

        private void BindTimelineTracks(List<GameObject> runners, GameObject boss)
        {
            timelineAsset = (TimelineAsset)m_startingCutscene.playableAsset;
            var outputs = timelineAsset.outputs;
            foreach (PlayableBinding itm in outputs)
            {
                if (itm.streamName.Contains("Move"))
                {
                    SetTrackBindingByTransform(itm, runners, boss);
                }
                else
                {
                    SentTrackBindingByObject(itm, runners, boss);
                }

            }
        }

        private void SetTrackBindingByTransform(PlayableBinding itm, List<GameObject> runners, GameObject boss)
        {
            if (itm.streamName.Contains("Player1") && runners.Count > 0)
            {
                m_startingCutscene.SetGenericBinding(itm.sourceObject, runners[0].transform);
            }
            else if (itm.streamName.Contains("Player2") && runners.Count > 1)
            {
                m_startingCutscene.SetGenericBinding(itm.sourceObject, runners[1].transform);
            }
            else if (itm.streamName.Contains("Player3") && runners.Count > 2)
            {
                m_startingCutscene.SetGenericBinding(itm.sourceObject, runners[2].transform);
            }
            else if (itm.streamName.Contains("Boss"))
            {
                m_startingCutscene.SetGenericBinding(itm.sourceObject, boss.transform);
            }
        }

        private void SentTrackBindingByObject(PlayableBinding itm, List<GameObject> runners, GameObject boss)
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

        private void UnbindTimelineTracks()
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
            Debug.LogError("This method should never be called");
        }
    }
}
