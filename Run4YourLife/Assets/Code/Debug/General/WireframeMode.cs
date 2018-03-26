using UnityEngine;

namespace Run4YourLife.DebuggingTools
{
    public class WireframeMode : MonoBehaviour
    {
        void OnPreRender()
        {
            GL.wireframe = true;
        }

        void OnPostRender()
        {
            GL.wireframe = false;
        }
    }
}