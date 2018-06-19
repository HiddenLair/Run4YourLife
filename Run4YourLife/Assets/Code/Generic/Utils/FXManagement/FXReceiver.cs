using UnityEngine;
using Run4YourLife.Utils;
using Run4YourLife.GameManagement.AudioManagement;

public class FXReceiver : MonoBehaviour {

    public GameObject fx;
    public Animator anim;
    public AudioClip sfx;

    public GameObject PlayFx(bool setAsParent = false)
    {
        if(sfx != null)
        {
            AudioManager.Instance.PlaySFX(sfx);
        }

        return FXManager.Instance.InstantiateFromReceiver(this, fx, setAsParent);
    }

    public void PlayFxOnAnimation(string animName,float time,bool setAsParent = false)
    {
        StartCoroutine(AnimationCallbacks.OnStateAtNormalizedTime(anim,animName,time,
            ()=>FXManager.Instance.InstantiateFromReceiver(this,fx,setAsParent))
        );
    }
}
