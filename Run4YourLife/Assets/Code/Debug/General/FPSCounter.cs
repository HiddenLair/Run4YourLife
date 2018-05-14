using UnityEngine;

namespace Run4YourLife.Debugging
{
    public class FPSCounter : DebugFeature
    {
        private const string cachedCurrentFpsHeaderStr = "Fps: ";
        private const string cachedCurrentMsHeaderStr = ", Ms: ";

        private string cachedCurrentFpsMsStr = string.Empty;

        private int lastFps = -1;
        private int lastMs = -1;

        protected override string GetPanelName()
        {
            return "Fps counter";
        }

        protected override void OnCustomDrawGUI()
        {
            bool modified = false;

            int currentFps = GetFPS();

            if(currentFps != lastFps)
            {
                modified = true;
                lastFps = currentFps;
            }

            int currentMs = GetMs();

            if(currentMs != lastMs)
            {
                modified = true;
                lastMs = currentMs;
            }

            if(modified)
            {
                cachedCurrentFpsMsStr = cachedCurrentFpsHeaderStr + currentFps + cachedCurrentMsHeaderStr + currentMs;
            }

            GUILayout.Label(cachedCurrentFpsMsStr);
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