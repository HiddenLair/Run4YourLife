using UnityEngine;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class Melee3 : Melee
    {
        #region Editor variables

        [SerializeField]
        private float instanceSpeed;

        [SerializeField]
        private Transform instancePos;

        #endregion

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

            GameObject meleeInst = Instantiate(instance, instancePos.position, Quaternion.identity);
            meleeInst.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, -instanceSpeed * Time.deltaTime, 0.0f);
        }
    }
}