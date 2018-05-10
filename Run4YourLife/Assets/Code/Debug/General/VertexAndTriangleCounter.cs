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

        protected override string GetPanelName()
        {
            return "Vertex and triangle counter";
        }

        protected override void OnCustomDrawGUI()
        {
            if((currentUpdateTimeS += Time.deltaTime) >= updateTimeS)
            {
                Count(); // Expensive
                currentUpdateTimeS = 0.0f;
            }

            GUILayout.Label("Num. Vertices: " + vertexCount);
            GUILayout.Label("Num. Triangles: " + triangleCount);
        }

        private void Count()
        {
            vertexCount = 0;
            triangleCount = 0;

            MeshRenderer[] meshes = FindObjectsOfType<MeshRenderer>();

            foreach(MeshRenderer mesh in meshes)
            {
                // Vertex and triangle count does not work with static game objects

                if(!mesh.gameObject.isStatic && mesh.isVisible)
                {
                    MeshFilter filter = mesh.GetComponent<MeshFilter>();

                    if(filter)
                    {
                        vertexCount += filter.sharedMesh.vertexCount;
                        triangleCount += filter.sharedMesh.triangles.Length; // ¡GC! O.o
                    }
                }
            }
        }
    }
}