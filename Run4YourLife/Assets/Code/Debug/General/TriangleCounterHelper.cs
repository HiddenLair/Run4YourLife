using UnityEngine;
using System.Collections.Generic;

namespace Run4YourLife.Debugging
{
    public class TriangleCounterHelper
    {
        private Dictionary<Mesh, int> triangles = new Dictionary<Mesh, int>();

        public int Get(Mesh mesh)
        {
            int numTriangles = 0;

            if(!triangles.TryGetValue(mesh, out numTriangles))
            {
                numTriangles = Count(mesh);
                triangles.Add(mesh, numTriangles);
            }

            return numTriangles;
        }

        public void Clear()
        {
            triangles.Clear();
        }

        private int Count(Mesh mesh)
        {
            return mesh.triangles.Length / 3;
        }
    }
}