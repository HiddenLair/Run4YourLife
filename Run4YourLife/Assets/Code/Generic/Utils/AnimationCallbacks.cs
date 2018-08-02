using System;
using System.Collections;

using UnityEngine;

namespace Run4YourLife.Utils
{
    public class AnimationCallbacks
    {

        ///<summary>
        /// When reaching the provided state at the provided normalized time
        ///</summary>
        public static IEnumerator OnStateAtNormalizedTime(Animator anim, string state, float normalizedTime, Action action, bool infinite = false)
        {
            WaitUntil waitUntilState = new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName(state) && Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime * anim.GetCurrentAnimatorStateInfo(0).speed, 1.0f) < normalizedTime);
            WaitUntil waitUntilNormalizedTime = new WaitUntil(() => Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime, 1.0f) >= normalizedTime);

            do
            {
                yield return waitUntilState;
                yield return waitUntilNormalizedTime;
                action.Invoke();
            } while (infinite);
        }

        ///<summary>
        /// When reaching the provided state
        ///</summary>
        public static IEnumerator OnState(Animator anim, string state, Action action, bool infinite = false)
        {
            WaitUntil waitUntilState = new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName(state));

            do
            {
                yield return waitUntilState;
                action.Invoke();
            } while (infinite);
        }

        ///<summary>
        /// After transitioning from the provided state at the provided normalized time
        ///</summary>
        public static IEnumerator AfterStateAtNormalizedTime(Animator anim, string state, float time, Action action, bool infinite = false)
        {
            WaitUntil waitUntilState = new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName(state));
            WaitUntil waitUntilTransition = new WaitUntil(() => anim.IsInTransition(0));          

            do
            {
                yield return waitUntilState;
                yield return waitUntilTransition;
                while (true)
                {
                    if (anim.IsInTransition(0))//While in transition, we need to look at normalized time in the next state, but when transition ends, the "next" state, is the current one
                    {
                        if(Mathf.Repeat(anim.GetNextAnimatorStateInfo(0).normalizedTime * anim.GetNextAnimatorStateInfo(0).speed, 1.0f) <= time)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime * anim.GetCurrentAnimatorStateInfo(0).speed, 1.0f) <= time)
                        {
                            break;
                        }
                    }
                    yield return null;
                }
                action.Invoke();
            } while (infinite);
        }

        ///<summary>
        /// After transitioning from the provided state to the provided state
        ///</summary>
        public static IEnumerator OnTransitionFromTo(Animator anim, string originState, string destinyState, Action action, bool infinite = false)
        {
            WaitUntil waitUntilStateOrigin = new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName(originState) && !anim.IsInTransition(0));
            WaitUntil waitUntilStateDestiny = new WaitUntil(() => anim.IsInTransition(0));
            do
            {
                while (true)
                {
                    yield return waitUntilStateOrigin;
                    yield return waitUntilStateDestiny;
                    if (anim.GetNextAnimatorStateInfo(0).IsName(destinyState))
                    {
                        action.Invoke();
                        break;
                    }
                }
            } while (infinite);
        }
    }
}
