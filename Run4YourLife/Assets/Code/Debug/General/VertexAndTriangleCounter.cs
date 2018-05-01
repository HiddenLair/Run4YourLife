using UnityEngine;

namespace Run4YourLife.Debug
{
    public class VertexAndTriangleCounter : DebugFeature
    {
        private int vertexCount = 0;
        private int triangleCount = 0;

        protected override string GetPanelName()
        {
            return "Vertex and triangle counter";
        }

        protected override void OnCustomDrawGUI()
        {
            Count();

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
                        triangleCount += filter.sharedMesh.triangles.Length;
                    }
                }
            }
        }
    }
}