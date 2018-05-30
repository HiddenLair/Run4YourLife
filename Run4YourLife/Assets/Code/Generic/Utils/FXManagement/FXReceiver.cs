using UnityEngine;
using Run4YourLife.Utils;

public class FXReceiver : MonoBehaviour {

    public GameObject fx;
    public Animator anim;

    public GameObject PlayFx(bool setAsParent = false)
    {
        return FXManager.Instance.InstantiateFromReceiver(this, fx, setAsParent);
    }

    public void PlayFxOnAnimation(string animName,float time,bool setAsParent = false)
    {
        StartCoroutine(AnimationCallback.OnStateAtNormalizedTime(anim,animName,time,
            ()=>FXManager.Instance.InstantiateFromReceiver(this,fx,setAsParent))
        );
    }
}
