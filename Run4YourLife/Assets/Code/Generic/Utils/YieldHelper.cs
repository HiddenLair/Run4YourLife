using System;
using UnityEngine;
using System.Collections;

namespace Run4YourLife.Utils
{
    public class YieldHelper
    {
        #region SkipFrame

        public static IEnumerator SkipFrame(Action action)
        {
            yield return null;
            action();
        }

        public static IEnumerator SkipFrames(Action action, int nFrames)
        {
            for (int i = 0; i < nFrames; i++)
            {
                yield return null;
            }

            action();
        }

        public static IEnumerator SkipFrame<T>(Action<T> action, T parameter)
        {
            yield return null;
            action(parameter);
        }

        #endregion

        #region WaitForSeconds

        public static IEnumerator WaitForSeconds(Action action, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            action();
        }

        public static IEnumerator WaitForSeconds<T>(Action<T> action, T parameter, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            action(parameter);
        }

        #endregion

        #region WaitUntil

        public static IEnumerator WaitUntil(Action action, Func<bool> predicate)
        {
            yield return new WaitUntil(predicate);
            action();
        }

        public static IEnumerator WaitUntil<T>(Action<T> action, T parameter, Func<bool> predicate)
        {
            yield return new WaitUntil(predicate);
            action(parameter);
        }

        #endregion
    }
}