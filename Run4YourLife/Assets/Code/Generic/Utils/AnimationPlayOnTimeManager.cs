using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationPlayOnTimeManager : SingletonMonoBehaviour<AnimationPlayOnTimeManager> {

	public void PlayOnAnimation(Animator anim,string animationName,float time, Action callBack)
    {
        StartCoroutine(WaitForCallBack(anim,animationName,time,callBack));
    }

    public void PlayOnNextAnimation(Animator anim, string animationName, float time, Action callBack)
    {
        StartCoroutine(WaitForNextCallback(anim,animationName,time,callBack));
    }

    public void PlayOnNextTransitionFrom(Animator anim, string firstAnimation,string secondAnimation,Action callBack)
    {
       StartCoroutine(WaitForEspecificTransition(anim, firstAnimation, secondAnimation, callBack));
    }

    IEnumerator WaitForCallBack(Animator anim, string animationName, float time, Action callBack)
    {
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName(animationName) && Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime * anim.GetCurrentAnimatorStateInfo(0).speed, 1.0f) < time);
        yield return new WaitUntil(() => Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime, 1.0f) >= time);
        callBack.Invoke();
    }

    IEnumerator WaitForNextCallback(Animator anim, string animationName, float time, Action callBack)
    {
        yield return new WaitUntil(()=>anim.GetCurrentAnimatorStateInfo(0).IsName(animationName));
        yield return new WaitUntil(()=>anim.IsInTransition(0));
        yield return new WaitUntil(() => Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime * anim.GetCurrentAnimatorStateInfo(0).speed, 1.0f) >= time);
        callBack.Invoke();
    }

    IEnumerator WaitForEspecificTransition(Animator anim, string firstStateName, string secondName,Action callBack)
    {
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName(firstStateName) && !anim.IsInTransition(0));
        yield return new WaitUntil(() => anim.IsInTransition(0)); 
        if (anim.GetNextAnimatorStateInfo(0).IsName(secondName))
        {
            callBack.Invoke();
        }
        else
        {
            StartCoroutine(WaitForEspecificTransition(anim,firstStateName,secondName,callBack));
        }
    }
}
