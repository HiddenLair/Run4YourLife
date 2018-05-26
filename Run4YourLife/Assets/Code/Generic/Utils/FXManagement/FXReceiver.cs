using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXReceiver : MonoBehaviour {

    public GameObject fx;
    public Animator anim;

    public GameObject PlayFx(bool setAsParent = false)
    {
        return FXManager.Instance.InstantiateFromReceiver(this, fx, setAsParent);
    }

    public void PlayFxOnAnimation(string animName,float time,bool setAsParent = false)
    {
        AnimationPlayOnTimeManager.Instance.PlayOnAnimation(anim,animName,time,()=>CallBack(fx,setAsParent));
    }

    void CallBack(GameObject prefab,bool setAsParent)
    {
        FXManager.Instance.InstantiateFromReceiver(this,prefab,setAsParent);
    }
}
