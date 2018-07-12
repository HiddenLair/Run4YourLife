using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Run4YourLife.Player;

namespace Run4YourLife.GameManagement
{
    public abstract class TransitionBase : GamePhaseManager
    {

        private TimelineAsset timelineAsset;

        protected void DeactivateScripts(GameObject g)
        {
            foreach (MonoBehaviour mono in g.GetComponents<MonoBehaviour>())
            {
                mono.enabled = false;
            }
        }

        protected void ActivateScripts(GameObject g)
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

        protected void BindTimelineTracks(PlayableDirector director, List<GameObject> runners, GameObject boss)
        {
            timelineAsset = (TimelineAsset)director.playableAsset;
            var outputs = timelineAsset.outputs;
            foreach (PlayableBinding itm in outputs)
            {
                if (itm.streamName.Contains("Animation"))
                {

                    if (CheckForGhost(itm, runners))
                    {
                        if (itm.streamName.Contains("Ghost"))
                        {
                            SentTrackBindingByObject(director, itm, runners, boss);
                        }
                    }
                    else
                    {
                        if (!itm.streamName.Contains("Ghost"))
                        {
                            SentTrackBindingByObject(director, itm, runners, boss);
                        }
                    }
                }
                else if (itm.streamName.Contains("Move"))
                {
                    SetTrackBindingByTransform(director, itm, runners, boss);
                }
                else
                {
                    SentTrackBindingByObject(director, itm, runners, boss);
                }
            }
        }

        protected void SetTrackBindingByTransform(PlayableDirector director, PlayableBinding itm, List<GameObject> runners, GameObject boss)
        {
            if (itm.streamName.Contains("Player1") && runners.Count > 0)
            {
                director.SetGenericBinding(itm.sourceObject, runners[0].transform);
            }
            else if (itm.streamName.Contains("Player2") && runners.Count > 1)
            {
                director.SetGenericBinding(itm.sourceObject, runners[1].transform);
            }
            else if (itm.streamName.Contains("Player3") && runners.Count > 2)
            {
                director.SetGenericBinding(itm.sourceObject, runners[2].transform);
            }
            else if (itm.streamName.Contains("Boss"))
            {
                director.SetGenericBinding(itm.sourceObject, boss.transform);
            }
        }

        protected void SentTrackBindingByObject(PlayableDirector director, PlayableBinding itm, List<GameObject> runners, GameObject boss)
        {
            if (itm.streamName.Contains("Player1") && runners.Count > 0)
            {
                director.SetGenericBinding(itm.sourceObject, runners[0]);
            }
            else if (itm.streamName.Contains("Player2") && runners.Count > 1)
            {
                director.SetGenericBinding(itm.sourceObject, runners[1]);
            }
            else if (itm.streamName.Contains("Player3") && runners.Count > 2)
            {
                director.SetGenericBinding(itm.sourceObject, runners[2]);
            }
            else if (itm.streamName.Contains("Boss"))
            {
                director.SetGenericBinding(itm.sourceObject, boss);
            }
        }

        private bool CheckForGhost(PlayableBinding itm, List<GameObject> runners)
        {
            if (itm.streamName.Contains("Player1") && runners.Count > 0)
            {
                if(runners[0].CompareTag(Tags.Ghost))
                {
                    return true;
                }
            }
            else if (itm.streamName.Contains("Player2") && runners.Count > 1)
            {
                if (runners[1].CompareTag(Tags.Ghost))
                {
                    return true;
                }
            }
            else if (itm.streamName.Contains("Player3") && runners.Count > 2)
            {
                if (runners[2].CompareTag(Tags.Ghost))
                {
                    return true;
                }
            }

            return false;
        }

        protected void Unbind(PlayableDirector director)
        {
            timelineAsset = (TimelineAsset)director.playableAsset;
            var outputs = timelineAsset.outputs;
            foreach (var itm in outputs)
            {
                director.SetGenericBinding(itm.sourceObject, null);
            }
        }
    }
}