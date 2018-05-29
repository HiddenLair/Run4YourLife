using UnityEngine;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Animator))]
    public class Melee1 : Melee
    {
        private Animator animator;

        protected override void Awake()
        {
            base.Awake();

            animator = GetComponent<Animator>();
        }

        protected override void OnSuccess()
        {
            animator.SetTrigger("Mele");
            AudioManager.Instance.PlaySFX(m_meleeClip);
        }
    }
}