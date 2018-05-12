using UnityEngine;

namespace Run4YourLife.Debugging
{
    public class VertexAndTriangleCounter : DebugFeature
    {
        [SerializeField]
        private float updateTimeS = 0.5f;

        private float currentUpdateTimeS = Mathf.Infinity;

        private int vertexCount = 0;
        private int triangleCount = 0;

        // Getting the triangle count of a mesh is expensive
        // TriangleCounterHelper minimizes this cost by caching this information

        private TriangleCounterHelper triangleCounterHelper = new TriangleCounterHelper();

        void OnDestroy()
        {
            triangleCounterHelper.Clear();
        }

        protected override string GetPanelName()
        {
            return "Vertex and triangle counter";
        }

        protected override void OnCustomDrawGUI()
        {
            if((currentUpdateTimeS += Time.deltaTime) >= updateTimeS)
            {
                Count();
                currentUpdateTimeS = 0.0f;
            }

            GUILayout.Label("Num. Vertices: " + vertexCount);
            GUILayout.Label("Num. Triangles: " + triangleCount);
        }

        private void Count() // Expensive
        {
            vertexCount = 0;
            triangleCount = 0;

            MeshRenderer[] meshes = FindObjectsOfType<MeshRenderer>();

            for(int i = 0; i < meshes.Length; ++i)
            {
                MeshRenderer meshRenderer = meshes[i];

                // Vertex and triangle count does not work with static game objects

                if(!meshRenderer.gameObject.isStatic && meshRenderer.isVisible)
                {
                    MeshFilter filter = meshRenderer.GetComponent<MeshFilter>();

                    if(filter != null)
                    {
                        Mesh mesh = filter.sharedMesh;

                        vertexCount += mesh.vertexCount;
                        triangleCount += triangleCounterHelper.Get(mesh);
                    }
                }
            }
        }
    }
}