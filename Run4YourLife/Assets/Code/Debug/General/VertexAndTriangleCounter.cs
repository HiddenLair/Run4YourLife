using UnityEngine;

namespace Run4YourLife.DebuggingTools
{
    public class VertexAndTriangleCounter : DebugFeature
    {
        private int vertexCount = 0;
        private int triangleCount = 0;

        public override void OnDrawGUI()
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