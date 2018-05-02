using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXReceiver : MonoBehaviour {

    public Animator anim;

    public GameObject PlayFx(GameObject prefab)
    {
        return FXManager.Instance.InstantiateFromReceiver(this, prefab);
    }

    public void PlayFxOnAnimation(string animName,float time, GameObject prefab)
    {
        AnimationPlayOnTime.Instance.PlayOnAnimation(anim,animName,time,()=>CallBack(prefab));
    }

    public void CallBack(GameObject prefab)
    {
        FXManager.Instance.InstantiateFromReceiver(this,prefab);
    }
}
