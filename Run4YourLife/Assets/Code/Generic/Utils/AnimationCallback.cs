using System;
using System.Collections;

using UnityEngine;

namespace Run4YourLife.Utils
{
    public class AnimationCallback : SingletonMonoBehaviour<AnimationCallback> {

        ///<summary>
        /// When reaching the provided state at the provided normalized time
        ///</summary>
        public static IEnumerator OnStateAtNormalizedTime(Animator anim, string state, float normalizedTime, Action action, bool infinite = false)
        {
            while(infinite)
            {
                yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName(state) && Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime * anim.GetCurrentAnimatorStateInfo(0).speed, 1.0f) < normalizedTime);
                yield return new WaitUntil(() => Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime, 1.0f) >= normalizedTime);
                action.Invoke();
            }
        }

        ///<summary>
        /// After transitioning from the provided state at the provided normalized time
        ///</summary>
        public static IEnumerator AfterStateAtNormalizedTime(Animator anim, string state, float time, Action action, bool infinite = false)
        {
            while(infinite)
            {
                yield return new WaitUntil(()=>anim.GetCurrentAnimatorStateInfo(0).IsName(state));
                yield return new WaitUntil(()=>anim.IsInTransition(0));
                yield return new WaitUntil(() => Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime * anim.GetCurrentAnimatorStateInfo(0).speed, 1.0f) >= time);
                action.Invoke();
            }
        }

        ///<summary>
        /// After transitioning from the provided state to the provided state
        ///</summary>
        public static IEnumerator OnTransitionFromTo(Animator anim, string originState, string destinyState, Action action, bool infinite = false)
        {
            while(infinite)
            {
                while (true)
                {
                    yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName(originState) && !anim.IsInTransition(0));
                    yield return new WaitUntil(() => anim.IsInTransition(0));
                    if (anim.GetNextAnimatorStateInfo(0).IsName(destinyState))
                    {
                        action.Invoke();
                        break;
                    }
                }
            }
        }
    }
}
