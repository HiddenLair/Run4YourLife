using UnityEngine;

namespace Run4YourLife.Debugging
{
    public class FPSCounter : DebugFeature
    {
        protected override string GetPanelName()
        {
            return "Fps counter";
        }

        protected override void OnCustomDrawGUI()
        {
            GUILayout.Label("Fps: " + GetFPS() + ", Ms: " + GetMs());
        }

        private int GetFPS()
        {
            return Mathf.RoundToInt(1.0f / Time.deltaTime);
        }

        private int GetMs()
        {
            return Mathf.RoundToInt(1000.0f * Time.deltaTime);
        }
    }
}