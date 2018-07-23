using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[Serializable]
public class ScreenFaderBehaviour : PlayableBehaviour
{
    public Color color = Color.black;
    public bool leaveColor = false;

    public float inverseDuration;

    public override void OnGraphStart(Playable playable)
    {
        double duration = playable.GetDuration();
        if (Mathf.Approximately((float)duration, 0f))
            throw new UnityException("A ScreenFader cannot have a duration of zero.");

        inverseDuration = 1f / (float)duration;
    }
 }
