using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafJumpOver : MonoBehaviour, JumpOver {

    [SerializeField]
    private float bounceOverMeForce;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    #region JumpOver
    public void JumpedOn()
    {
        anim.SetTrigger("bump");
    }

    public float GetBounceForce()
    {
        return bounceOverMeForce;
    }
    #endregion
}
