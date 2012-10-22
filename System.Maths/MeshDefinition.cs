using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Maths
{
    public class MeshDefinition
    {
        List<Vertex> vertexes = new List<Vertex>();

        List<int> indices = new List<int>();

        public void AddVertex(Vertex v)
        {
            vertexes.Add(v);
        }

        public void AddTriangle(int v1, int v2, int v3)
        {
            indices.Add(v1);
            indices.Add(v2);
            indices.Add(v3);
        }

        public IEnumerable<Triangle> Triangles
        {
            get
            {
                for (int i = 0; i < indices.Count / 3; i++)
                {
                    int v1 = indices[i];
                    int v2 = indices[i + 1];
                    int v3 = indices[i + 2];

                    if (v1 >= 0 && v1 < vertexes.Count && v2 >= 0 && v2 < vertexes.Count && v3 >= 0 && v3 < vertexes.Count)
                        yield return new Triangle(vertexes[v1].Position, vertexes[v2].Position, vertexes[v3].Position);
                }
            }
        }
    }

    public struct Vertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector3 Tangent;
        public Vector3 Binormal;

        public Vector3 TextureCoordinates;
    }
}
