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

        private Coroutine m_startPhaseCoroutine;

        public override void StartPhase()
        {
            m_startPhaseCoroutine = StartCoroutine(StartPhaseCoroutine());
        }

        private IEnumerator StartPhaseCoroutine()
        {
            StartCutScene();

            yield return new WaitUntil(() => m_startingCutscene.state != PlayState.Playing); // wait until cutscene has completed

            EndCutScene();

            GameManager.Instance.EndGame_BossWin();
        }     

        private void StartCutScene()
        {
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            List<GameObject> ghosts = GameplayPlayerManager.Instance.Ghosts;
            GameplayPlayerManager.Instance.SetEventsToListen(false);

            foreach (GameObject ghost in ghosts)
            {
                DeactivateScripts(ghost);
            }

            DeactivateScripts(boss);

            BindTimelineTracks(m_startingCutscene, ghosts, boss);
            BindEndLocationTransformReference(boss.GetComponent<AbsorbPartGetter>().GetPartToAbsorb(), "Move", 0, 0);
            m_startingCutscene.Play();
        }

        private void EndCutScene()
        {
            m_startingCutscene.Stop();
            Unbind(m_startingCutscene);
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
                            clip.endLocation.exposedName = UnityEngine.Random.Range(float.MinValue,float.MaxValue).ToString();
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
            StopCoroutine(m_startPhaseCoroutine);
            m_startPhaseCoroutine = null;

            EndCutScene();
            foreach (GameObject ghost in GameplayPlayerManager.Instance.Ghosts)
            {
                ActivateScripts(ghost);
            }
            ActivateScripts(GameplayPlayerManager.Instance.Boss);

            GameplayPlayerManager.Instance.DebugClearPlayers();
        }
    }
}
