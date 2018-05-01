using UnityEngine;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class Melee3 : Melee
    {
        private Animator animator;
        private AudioSource audioSource;

        protected override void GetComponents()
        {
            base.GetComponents();

            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        protected override void OnSuccess()
        {
            animator.SetTrigger("Mele");
            audioSource.PlayOneShot(sfx);
        }
    }
}