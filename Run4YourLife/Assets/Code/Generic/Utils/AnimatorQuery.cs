using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Utils
{
    public class AnimatorQuery  {

        public static bool IsInStateCompletely(Animator animator, string state)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(state) && !animator.IsInTransition(0);
        }
    }
}
