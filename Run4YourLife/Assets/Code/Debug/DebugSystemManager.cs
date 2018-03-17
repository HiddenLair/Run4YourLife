using UnityEngine;

namespace Run4YourLife.DebuggingTools
{
    public class DebugSystemManager : MonoBehaviour
    {
        private GameObject mainCamera = null;

        private VertexAndTriangleCounter vertexAndTriangleCounter = null;

        private WireframeMode wireframeMode = null;

        private FPSCounter fpsCounter = null;

        void Awake()
        {
            mainCamera = Camera.main.gameObject;
        }

        void Update()
        {
            if(UnityEngine.Input.GetKeyDown(KeyCode.F1))
            {
                fpsCounter = ToggleMode<FPSCounter>(fpsCounter);
            }

            if(UnityEngine.Input.GetKeyDown(KeyCode.F2))
            {
                vertexAndTriangleCounter = ToggleMode<VertexAndTriangleCounter>(vertexAndTriangleCounter);
            }

            if(UnityEngine.Input.GetKeyDown(KeyCode.F3))
            {
                wireframeMode = ToggleMode<WireframeMode>(wireframeMode);
            }
        }

        void OnGUI()
        {
            DrawFPSCounter();
            DrawVertexAndTriangleCounter();
        }

        private T ToggleMode<T>(MonoBehaviour mode) where T : Component
        {
            T ret = null;

            if(mode == null)
            {
                ret = mainCamera.AddComponent<T>();
            }
            else
            {
                Destroy(mode);
            }

            return ret;
        }

        private void DrawFPSCounter()
        {
            if(fpsCounter != null)
            {
                float w = 150.0f;
                float h = 20.0f;

                float x = Screen.width - w;
                float y = 10.0f;

                GUI.Label(new Rect(x, y, w, h), "FPS: " + fpsCounter.GetFPS().ToString("0.###"));
            }
        }

        private void DrawVertexAndTriangleCounter()
        {
            if(vertexAndTriangleCounter != null)
            {
                float w = 150.0f;
                float h = 20.0f;

                float x = Screen.width - w;

                float vY = 30.0f;
                float vT = vY + h;

                GUI.Label(new Rect(x, vY, w, h), "Num. Vertices: " + vertexAndTriangleCounter.GetVertexCount());

                GUI.Label(new Rect(x, vT, w, h), "Num. Triangles: " + vertexAndTriangleCounter.GetTriangleCount());
            }
        }
    }
}