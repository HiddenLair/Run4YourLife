using System;
using UnityEngine;
using UnityEngine.Playables;
using Run4YourLife.GameManagement;


[Serializable]
public class TrembleBehaviour : PlayableBehaviour
{
    public TrembleConfig trembleConfig;
    public bool useDuration;

    public override void OnGraphStart(Playable playable)
    {
        if (Application.isPlaying)
        {
            trembleConfig.duration = (float)playable.GetDuration();
            trembleConfig.useDuration = useDuration;
        }
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (Application.isPlaying)
        {
            base.OnBehaviourPlay(playable, info);
            TrembleManager.Instance.Tremble(trembleConfig);
        }
    }
}
