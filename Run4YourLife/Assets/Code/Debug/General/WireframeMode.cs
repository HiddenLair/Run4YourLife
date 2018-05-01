using UnityEngine;

namespace Run4YourLife.Debug
{
    public class WireframeMode : DebugFeature
    {
        private bool wireframe = false;

        protected override string GetPanelName()
        {
            return "Wireframe mode";
        }

        protected override void OnCustomDrawGUI()
        {
            wireframe = GUILayout.Toggle(wireframe, " Active");
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