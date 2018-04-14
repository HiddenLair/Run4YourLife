using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.Player;

namespace Run4YourLife.Interactables
{
    public class LeafJumpOver : RunnerBounceDetection
    {

        [SerializeField]
        private Vector3 direction;

        [SerializeField]
        private float bounceOverMeForce;

        private Animator anim;

        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
        }

        #region JumpOver
        protected override void JumpedOn()
        {
            anim.SetTrigger("bump");
        }

        protected override Vector3 GetBounceForce()
        {
            return bounceOverMeForce * direction.normalized;
        }
        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + direction.normalized * 5);
        }
    }
}
