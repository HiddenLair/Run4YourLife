using UnityEngine;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Animator))]
    public class Ready : MonoBehaviour
    {
        #region Editor variables

        [SerializeField]
        private string animationReadyName = "move";

        #endregion

        private bool ready = false;

        private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            ready = animator.GetCurrentAnimatorStateInfo(0).IsName(animationReadyName) && animator.GetNextAnimatorClipInfoCount(0) == 0;
        }

        public bool Get()
        {
            return ready;
        }
    }
}