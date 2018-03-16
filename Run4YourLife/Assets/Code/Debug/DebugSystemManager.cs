using UnityEngine;

namespace Run4YourLife.DebuggingTools
{
    public class DebugSystemManager : MonoBehaviour
    {
        private GameObject mainCamera = null;

        private VertexAndTriangleCounter vertexAndTriangleCounter = null;

        private WireframeMode wireframeMode = null;

        void Awake()
        {
            mainCamera = Camera.main.gameObject;
        }

        void Update()
        {
            if(UnityEngine.Input.GetKeyDown(KeyCode.F1))
            {
                EnableVertexAndTriangleCounter(vertexAndTriangleCounter == null);
            }

            if(UnityEngine.Input.GetKeyDown(KeyCode.F2))
            {
                EnableWireframeMode(wireframeMode == null);
            }
        }

        void OnGUI()
        {
            DrawVertexAndTriangleCounter();
        }

        private void EnableVertexAndTriangleCounter(bool enabled)
        {
            if(enabled && !vertexAndTriangleCounter)
            {
                vertexAndTriangleCounter = mainCamera.AddComponent<VertexAndTriangleCounter>();
            }
            else if(!enabled && vertexAndTriangleCounter)
            {
                Destroy(vertexAndTriangleCounter);
                vertexAndTriangleCounter = null;
            }
        }

        private void EnableWireframeMode(bool enabled)
        {
            if(enabled && !wireframeMode)
            {
                wireframeMode = mainCamera.AddComponent<WireframeMode>();
            }
            else if(!enabled && wireframeMode)
            {
                Destroy(wireframeMode);
                wireframeMode = null;
            }
        }

        private void DrawVertexAndTriangleCounter()
        {
            if(vertexAndTriangleCounter != null)
            {
                float w = 150.0f;
                float h = 20.0f;

                float x = Screen.width - w;

                float vY = 10.0f;
                float vT = vY + h;

                GUI.Label(new Rect(x, vY, w, h), "Num. Vertices: " + vertexAndTriangleCounter.GetVertexCount());

                GUI.Label(new Rect(x, vT, w, h), "Num. Triangles: " + vertexAndTriangleCounter.GetTriangleCount());
            }
        }
    }
}