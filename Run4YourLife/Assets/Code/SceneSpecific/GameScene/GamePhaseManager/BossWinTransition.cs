using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Run4YourLife.Player;
using System;

namespace Run4YourLife.GameManagement
{
    public class BossWinTransition : TransitionBase
    {
        public override GamePhase GamePhase { get { return GamePhase.BossWin; } }

        [SerializeField]
        private PlayableDirector m_startingCutscene;

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

            BindTimelineTracks(m_startingCutscene,ghosts, boss);
            BindEndLocationTransformReference(boss.GetComponent<AbsorbPartGetter>().GetPartToAbsorb(),"Move",0,0);
            m_startingCutscene.Play();

            yield return new WaitUntil(() => m_startingCutscene.state != PlayState.Playing); // wait until cutscene has completed

            Unbind(m_startingCutscene);

            GameManager.Instance.EndGame_BossWin();
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
