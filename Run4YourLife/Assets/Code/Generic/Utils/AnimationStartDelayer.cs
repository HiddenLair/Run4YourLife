using UnityEngine;

namespace Run4YourLife.Utils
{
    public class AnimationStartDelayer : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private string AnimatorStateName;

        [SerializeField]
        private float delay = 0.0f;

        [SerializeField]
        private bool randomize = false;

        void Start()
        {
            float delayTime;

            if(randomize)
            {
                delayTime = Random.Range(0.0f, 1.0f * delay);
            }
            else
            {
                delayTime = delay;
            }

            animator.Play(AnimatorStateName, -1, delayTime);
        }
    }
}
