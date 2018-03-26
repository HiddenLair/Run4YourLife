using UnityEngine;

namespace Run4YourLife.DebuggingTools
{
    public class VertexAndTriangleCounter : MonoBehaviour
    {
        private int vertexCount = 0;
        private int triangleCount = 0;

        void Update()
        {
            Count();
        }

        public int GetVertexCount()
        {
            return vertexCount;
        }

        public int GetTriangleCount()
        {
            return triangleCount;
        }

        private void Count()
        {
            vertexCount = 0;
            triangleCount = 0;

            MeshRenderer[] meshes = FindObjectsOfType<MeshRenderer>();

            foreach(MeshRenderer mesh in meshes)
            {
                if(mesh.isVisible)
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