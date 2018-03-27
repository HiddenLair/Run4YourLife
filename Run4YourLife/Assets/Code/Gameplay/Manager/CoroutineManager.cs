using UnityEngine;

namespace Run4YourLife.GameManagement
{
    public class CoroutineManager : MonoBehaviour
    {
        private static CoroutineManager coroutineManager = null;

        void Awake()
        {
            coroutineManager = this;
        }

        public static CoroutineManager GetInstance()
        {
            return coroutineManager;
        }
    }
}