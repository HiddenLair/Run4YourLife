using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.Player;

namespace Run4YourLife.Interactables
{
    public class BounceableEntityController : BounceableEntityBase
    {
        [SerializeField]
        [Range(0,360)]
        private float m_angle;

        [SerializeField]
        private float m_bounceForce;

        private Animator anim;

        protected override void Awake()
        {
            base.Awake();
            anim = GetComponentInChildren<Animator>();
        }

        protected override void BouncedOn()
        {
            anim.SetTrigger("bump");
        }

        protected override Vector3 BounceForce
        {
            get
            {
                return m_bounceForce * (Quaternion.Euler(0, 0, m_angle) * Vector3.right);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + BounceForce);
        }
    }
}
