using UnityEngine;


namespace Run4YourLife.Debugging
{
    public class VertexAndTriangleCounter : DebugFeature
    {
#if UNITY_EDITOR
        [SerializeField]
        private float updateTimeS = 0.5f;

        private const string cachedCurrentVertexHeaderStr = "Num. Vertices: ";
        private const string cachedCurrentTriangleHeaderStr = "Num. Triangles: ";

        private string cachedCurrentVertexStr = string.Empty;
        private string cachedCurrentTriangleStr = string.Empty;

        private float currentUpdateTimeS = Mathf.Infinity;

        private int lastVertexCount = -1;
        private int lastTriangleCount = -1;

        private int currentVertexCount = 0;
        private int currentTriangleCount = 0;

        // Getting the triangle count of a mesh is expensive
        // TriangleCounterHelper minimizes this cost by caching this information

        private TriangleCounterHelper triangleCounterHelper = new TriangleCounterHelper();
#endif

        void OnDestroy()
        {
#if UNITY_EDITOR
            triangleCounterHelper.Clear();
#endif
        }

        protected override string GetPanelName()
        {
#if UNITY_EDITOR
            return "Vertex and triangle counter (only in editor)";
#else
            return "Vertex and triangle counter";
#endif
        }

        protected override void OnCustomDrawGUI()
        {
#if UNITY_EDITOR
            if ((currentUpdateTimeS += Time.deltaTime) >= updateTimeS)
            {
                Count(); // Updates current and last values
                currentUpdateTimeS = 0.0f;

                if (currentVertexCount != lastVertexCount)
                {
                    cachedCurrentVertexStr = cachedCurrentVertexHeaderStr + currentVertexCount;
                }

                if (currentTriangleCount != lastTriangleCount)
                {
                    cachedCurrentTriangleStr = cachedCurrentTriangleHeaderStr + currentTriangleCount;
                }
            }

            GUILayout.Label(cachedCurrentVertexStr);
            GUILayout.Label(cachedCurrentTriangleStr);
#endif
        }

#if UNITY_EDITOR
        private void Count() // Expensive
        {
            lastVertexCount = currentVertexCount;
            lastTriangleCount = currentTriangleCount;

            currentVertexCount = 0;
            currentTriangleCount = 0;

            MeshRenderer[] meshes = FindObjectsOfType<MeshRenderer>();

            for (int i = 0; i < meshes.Length; ++i)
            {
                MeshRenderer meshRenderer = meshes[i];

                // Vertex and triangle count does not work with static game objects

                if (!meshRenderer.gameObject.isStatic && meshRenderer.isVisible)
                {
                    MeshFilter filter = meshRenderer.GetComponent<MeshFilter>();

                    if (filter != null)
                    {
                        Mesh mesh = filter.sharedMesh;
                        if (mesh != null)
                        {
                            currentVertexCount += mesh.vertexCount;
                            currentTriangleCount += triangleCounterHelper.Get(mesh);
                        }
                        else
                        {
                            Debug.LogWarning("Mesh filter on GameObject: <" + filter.gameObject.name + "> has a null mesh", filter.gameObject);
                        }
                    }
                }
            }
        }
#endif
    }
}