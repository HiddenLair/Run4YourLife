using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.8455882f, 0.6140376f, 0.006217566f)]
[TrackClipType(typeof(TrembleClip))]
public class TrembleTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<TrembleMixerBehaviour>.Create (graph, inputCount);
    }
}
