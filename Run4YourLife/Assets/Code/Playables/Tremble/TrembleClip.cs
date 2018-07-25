using System;

using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TrembleClip : PlayableAsset, ITimelineClipAsset
{
    public TrembleBehaviour template = new TrembleBehaviour();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TrembleBehaviour>.Create(graph, template);
        TrembleBehaviour clone = playable.GetBehaviour();
        return playable;
    }
}
