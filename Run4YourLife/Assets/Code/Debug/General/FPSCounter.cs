using UnityEngine;

namespace Run4YourLife.DebuggingTools
{
    public class FPSCounter : DebugFeature
    {
        public override void OnDrawGUI()
        {
            GUILayout.Label("Fps: " + GetFPS().ToString("0") + ", Ms: " + GetMs().ToString("0"));
        }

        private float GetFPS()
        {
            return 1.0f / Time.deltaTime;
        }

        private float GetMs()
        {
            return 1000.0f * Time.deltaTime;
        }
    }
}