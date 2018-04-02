using UnityEngine;

namespace Run4YourLife.DebuggingTools
{
    public class WireframeMode : DebugFeature
    {
        private bool wireframe = false;

        public override void OnDrawGUI()
        {
            wireframe = GUILayout.Toggle(wireframe, " Wireframe Mode");
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