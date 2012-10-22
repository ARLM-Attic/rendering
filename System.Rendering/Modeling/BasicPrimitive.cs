using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Maths;

namespace System.Rendering.Modeling
{
    /// <summary>
    /// Represents a basic primitive formed by a secuence of vertices and connections between them.
    /// </summary>
    public struct Basic : IVertexedGraphicPrimitive, IIntersectableGraphicPrimitive
    {
        /// <summary>
        /// BasicPrimitiveType specifying the connections and filling between vertexes.
        /// </summary>
        public readonly BasicPrimitiveType Type;
        /// <summary>
        /// Secuence of vertexes used to form the primitive.
        /// </summary>
        public readonly VertexBuffer VertexBuffer;

        public readonly IndexBuffer Indexes;

        public readonly int StartIndex;

        public readonly int Count;

        /// <summary>
        /// Initialize a BasicPrimitive object using a BasicPrimitiveType value and a set of vertexes.
        /// </summary>
        /// <param name="type">BasicPrimitiveType to specify how vertexes are used to form the primitive</param>
        /// <param name="buffer">Secuence of vertexes of the primitive</param>
        /// <param name="indexes">Array of int representing the indexes of vertexes, if this value is null, default order is assumed.</param>
        private Basic(BasicPrimitiveType type, VertexBuffer buffer, IndexBuffer indexes)
        {
            this.Type = type;
            this.VertexBuffer = buffer;
            this.Indexes = indexes;
            this.lastIndexVersion = -1;
            this.lastVertexVersion = -1;
            this.positions = null;
            this.indexes = null;

            this.StartIndex = 0;
            this.Count = indexes == null ? buffer.Length : indexes.Length;
        }
        private Basic(BasicPrimitiveType type, VertexBuffer buffer, IndexBuffer indexes, int start, int count)
        {
            this.Type = type;
            this.VertexBuffer = buffer;
            this.Indexes = indexes;
            this.lastIndexVersion = -1;
            this.lastVertexVersion = -1;
            this.positions = null;
            this.indexes = null;

            this.StartIndex = start;
            this.Count = count;
        }

        private static IEnumerable<Vector3> UnrollTriangleFan(PositionData[] vertexes, int[] indexes)
        {
            if (indexes == null)
                for (int i = 2; i < vertexes.Length; i++)
                {
                    yield return vertexes[0].Position;
                    yield return vertexes[i - 1].Position;
                    yield return vertexes[i].Position;
                }
            else
                for (int i = 2; i < indexes.Length; i++)
                {
                    yield return vertexes[indexes[0]].Position;
                    yield return vertexes[indexes[i - 1]].Position;
                    yield return vertexes[indexes[i]].Position;
                }
        }

        private static IEnumerable<Vector3> UnrollTriangleStrip(PositionData[] vertexes, int[] indexes)
        {
            if (indexes == null)
                for (int i = 2; i < vertexes.Length; i++)
                {
                    yield return vertexes[i - 2].Position;
                    yield return vertexes[i - 1].Position;
                    yield return vertexes[i].Position;
                }
            else
                for (int i = 2; i < indexes.Length; i++)
                {
                    yield return vertexes[indexes[i - 2]].Position;
                    yield return vertexes[indexes[i - 1]].Position;
                    yield return vertexes[indexes[i]].Position;
                }
        }

        private static IEnumerable<Vector3> UnrollTriangleList(PositionData[] vertexes, int[] indexes)
        {
            if (indexes == null)
            {
                Vector3[] positions = new Vector3[vertexes.Length];
                for (int i = 0; i < vertexes.Length; i++)
                    positions[i] = vertexes[i].Position;
                return positions;
            }
            else
            {
                Vector3[] positions = new Vector3[indexes.Length];
                for (int i = 0; i < indexes.Length; i++)
                    positions[i] = vertexes[indexes[i]].Position;
                return positions;
            }
        }

        private static Triangle[] UnrollTriangles(PositionData[] vertexes, int[] indexes, BasicPrimitiveType primitiveType)
        {
            Vector3[] positions = null;
            switch (primitiveType)
            {
                case BasicPrimitiveType.Triangle_Fan:
                    positions = UnrollTriangleFan(vertexes, indexes).ToArray();
                    break;
                case BasicPrimitiveType.Triangle_Strip:
                    positions = UnrollTriangleStrip(vertexes, indexes).ToArray();
                    break;
                case BasicPrimitiveType.Triangles:
                    positions = (Vector3[])UnrollTriangleList(vertexes, indexes);
                    break;
            }

            Triangle[] triangles = new Triangle[positions.Length / 3];
            for (int i = 0; i < triangles.Length; i++)
                triangles[i] = new Triangle(positions[i * 3 + 0], positions[i * 3 + 1], positions[i * 3 + 2]);

            return triangles;
        }

        long lastVertexVersion;
        PositionData[] positions;

        long lastIndexVersion;
        int[] indexes;

        internal Triangle[] GetTriangles()
        {
            switch (Type)
            {
                case BasicPrimitiveType.Line_Loop:
                case BasicPrimitiveType.Line_Strip:
                case BasicPrimitiveType.Lines:
                case BasicPrimitiveType.Points:
                case BasicPrimitiveType.Tetraedron:
                case BasicPrimitiveType.Box:
                    return new Triangle[0];
                case BasicPrimitiveType.Triangles:
                case BasicPrimitiveType.Triangle_Strip:
                case BasicPrimitiveType.Triangle_Fan:
                    if (VertexBuffer.Version != lastVertexVersion)
                    {
                        positions = (VertexBuffer.Clone<VertexBuffer, PositionData>()).DirectData as PositionData[];
                        lastVertexVersion = VertexBuffer.Version;
                    }

                    if (Indexes != null && Indexes.Version != lastIndexVersion || Indexes == null)
                    {
                        indexes = Indexes == null ? null : (Indexes.Clone<IndexBuffer, int>()).DirectData as int[];
                    }

                    return UnrollTriangles(positions, indexes, Type);
            }
            return new Triangle[0];
        }

        public IntersectInfo[] GetIntersections(Ray ray)
        {
            List<IntersectInfo> intersections = new List<IntersectInfo>();

            foreach (var triangle in GetTriangles())
            {
                IntersectInfo info = null;
                if (triangle.Intersect(ray, out info))
                    intersections.Add(info);
            }

            intersections.Sort();

            return intersections.ToArray();
        }

        public static Basic Create(BasicPrimitiveType type, VertexBuffer vertexBuffer, IndexBuffer indexBuffer)
        {
            return new Basic(type, vertexBuffer, indexBuffer);
        }

        public static Basic Create(BasicPrimitiveType type, VertexBuffer vertexBuffer, int startVertex, int vertexCount)
        {
            return new Basic(type, vertexBuffer, null, startVertex, vertexCount);
        }

        public static Basic Create(BasicPrimitiveType type, VertexBuffer vertexBuffer, IndexBuffer indexBuffer, int startIndex, int count)
        {
            return new Basic(type, vertexBuffer, indexBuffer, startIndex, count);
        }

        public static Basic Create<FVF>(BasicPrimitiveType type, params FVF[] vertexes) where FVF : struct
        {
            return new Basic(type, vertexes, null);
        }

        public static Basic Triangles<FVF>(IEnumerable<FVF> buffer) where FVF :struct
        {
            return new Basic(BasicPrimitiveType.Triangles, buffer.ToArray (), null);
        }

        public static Basic Triangles<FVF>(params FVF[] buffer) where FVF : struct
        {
            return new Basic(BasicPrimitiveType.Triangles, buffer, null);
        }

        public static Basic TriangleStrip<FVF>(IEnumerable<FVF> buffer) where FVF : struct
        {
            return new Basic(BasicPrimitiveType.Triangle_Strip, buffer.ToArray(), null);
        }

        public static Basic TriangleStrip<FVF>(params FVF[] buffer) where FVF : struct
        {
            return new Basic(BasicPrimitiveType.Triangle_Strip, buffer, null);
        }

        public static Basic TriangleFan<FVF>(IEnumerable<FVF> buffer) where FVF : struct
        {
            return new Basic(BasicPrimitiveType.Triangle_Fan, buffer.ToArray(), null);
        }

        public static Basic TriangleFan<FVF>(params FVF[] buffer) where FVF : struct
        {
            return new Basic(BasicPrimitiveType.Triangle_Fan, buffer, null);
        }

        public static Basic Lines<FVF>(IEnumerable<FVF> buffer) where FVF : struct
        {
            return new Basic(BasicPrimitiveType.Lines, buffer.ToArray(), null);
        }

        public static Basic Lines<FVF>(params FVF[] buffer) where FVF : struct
        {
            return new Basic(BasicPrimitiveType.Lines, buffer, null);
        }

        public static Basic LineLoop<FVF>(IEnumerable<FVF> buffer) where FVF : struct
        {
            return new Basic(BasicPrimitiveType.Line_Loop, buffer.ToArray(), null);
        }

        public static Basic LinesLoop<FVF>(params FVF[] buffer) where FVF : struct
        {
            return new Basic(BasicPrimitiveType.Line_Loop, buffer, null);
        }

        public static Basic LineStrip<FVF>(IEnumerable<FVF> buffer) where FVF : struct
        {
            return new Basic(BasicPrimitiveType.Line_Strip, buffer.ToArray(), null);
        }

        public static Basic LineStrip<FVF>(params FVF[] buffer) where FVF : struct
        {
            return new Basic(BasicPrimitiveType.Line_Strip, buffer, null);
        }

        public static Basic Points<FVF>(IEnumerable<FVF> buffer) where FVF : struct
        {
            return new Basic(BasicPrimitiveType.Points, buffer.ToArray(), null);
        }

        public static Basic Points<FVF>(params FVF[] buffer) where FVF : struct
        {
            return new Basic(BasicPrimitiveType.Points, buffer, null);
        }

        public IntersectInfo[] Intersect(Ray ray)
        {
            return GetIntersections(ray);
        }

        VertexBuffer IVertexedGraphicPrimitive.VertexBuffer
        {
            get { return VertexBuffer; }
        }
    }

    /// <summary>
    /// Defines supported basic primitive types for Boundary, Wireframes and Points Representation.
    /// </summary>
    public enum BasicPrimitiveType
    {
        /// <summary>
        /// Vertexes are drawn as simple points
        /// </summary>
        Points,
        /// <summary>
        /// Vertexes are used to draw lines by pairs.
        /// </summary>
        Lines,
        /// <summary>
        /// Vertexes are used to draw a line starting in the first and ending in the lastone.
        /// </summary>
        Line_Strip,
        /// <summary>
        /// Vertexes are used to draw a line starting and ending in the first passing throught all the others.
        /// </summary>
        Line_Loop,
        /// <summary>
        /// Vertexes are used in group of 3 to draw triangles
        /// </summary>
        Triangles,
        /// <summary>
        /// Vertexes are used to draw a strip of triangles
        /// </summary>
        Triangle_Strip,
        /// <summary>
        /// Vertexes are used to draw a fan of triangles where first vertex is shared.
        /// </summary>
        Triangle_Fan,
        /// <summary>
        /// Vertexes grouped by 4 are used to draw a volumetric cell [This primitive is not supported for several APIs.]
        /// </summary>
        Tetraedron,
        /// <summary>
        /// Vertexes grouped by 8 are used to draw a volumetric cell [This primitive is not supported for several APIs.]
        /// </summary>
        Box
    }
}
