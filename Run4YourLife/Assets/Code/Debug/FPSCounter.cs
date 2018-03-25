using UnityEngine;

namespace Run4YourLife.DebuggingTools
{
    public class FPSCounter : MonoBehaviour
    {
        public float GetFPS()
        {
            return 1.0f / Time.deltaTime;
        }

        public float GetMs()
        {
            return 1000.0f * Time.deltaTime;
        }
    }
}