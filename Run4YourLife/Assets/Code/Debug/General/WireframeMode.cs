using UnityEngine;

namespace Run4YourLife.Debugging
{
    public class WireframeMode : DebugFeature
    {
        private const string cachedToggleTextStr = " Active";

        private bool wireframe = false;

        protected override string GetPanelName()
        {
            return "Wireframe mode";
        }

        protected override void OnCustomDrawGUI()
        {
            wireframe = GUILayout.Toggle(wireframe, cachedToggleTextStr);
        }

        void OnPreRender()
        {
            GL.wireframe = wireframe;
        }

        void OnPostRender()
        {
            GL.wireframe = false;
        }
    }
}