using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXReceiver : MonoBehaviour {

    public GameObject fx;
    public Animator anim;

    public GameObject PlayFx()
    {
        return FXManager.Instance.InstantiateFromReceiver(this, fx);
    }

    public void PlayFxOnAnimation(string animName,float time)
    {
        AnimationPlayOnTimeManager.Instance.PlayOnAnimation(anim,animName,time,()=>CallBack(fx));
    }

    void CallBack(GameObject prefab)
    {
        FXManager.Instance.InstantiateFromReceiver(this,prefab);
    }
}
