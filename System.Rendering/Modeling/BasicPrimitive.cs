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
        internal class Triangle
        {
            public Vector3 V1, V2, V3;
            public Vector3 N1, N2, N3;

            public Triangle(Vector3 V1, Vector3 V2, Vector3 V3, Vector3 N1, Vector3 N2, Vector3 N3)
            {
                this.V1 = V1; 
                this.V2 = V2;
                this.V3 = V3;
                this.N1 = N1;
                this.N2 = N2;
                this.N3 = N3;
            }

            public Vector3 Normal
            {
                get
                {
                    Vector3 U = V3 - V1;
                    Vector3 V = V2 - V1;
                    return GMath.normalize(GMath.cross(V, U));
                }
            }

            public Vector3 ClosestTo(Vector3 target)
            {
                Vector3 v1 = this.V2 - this.V1;
                Vector3 v2 = this.V3 - this.V1;
                Vector3 v = target - this.V1;

                if (GMath.dot(v1, v1) == 0)
                {
                    Vector3 temp = v2;
                    v2 = v1;
                    v1 = temp;
                }

                float a11 = GMath.dot(v1, v1);
                float a12 = GMath.dot(v1, v2);
                float a13 = GMath.dot(v1, v);
                float a21 = GMath.dot(v2, v1);
                float a22 = GMath.dot(v2, v2);
                float a23 = GMath.dot(v2, v);

                if (a11 == 0)
                    return target;

                a23 = a13 * a21 - a23 * a11;
                a22 = a12 * a21 - a22 * a11;
                a21 = 0;

                if (a22 == 0)
                    return target;
                float beta = a23 / a22;
                float alpha = (a13 - a12 * beta) / a11;

                return (v1 * alpha + v2 * beta) + this.V1;
            }

            public Vector3 Reflect(Vector3 target)
            {
                Vector3 proj = ClosestTo(target);
                return (proj - target) + proj;
            }

            bool Inside(Ray ray, Vector3 P1, Vector3 P2)
            {
                Vector3 v1 = P1 - ray.Position;
                Vector3 v2 = P2 - ray.Position;
                Vector3 normal = GMath.cross(v1, v2);
                return GMath.dot(normal, ray.Direction) >= 0;
            }

            public bool Intersect(Ray ray, out IntersectInfo info)
            {
                info = null;
                if (Intersect(ray))
                    return IntersectPlane(ray, out info) && info.U >= 0 && info.V >= 0 && info.U + info.V <= 1;
                return false;
            }

            public bool IntersectPlane(Ray ray, out IntersectInfo info)
            {
                float U = 0;
                float V = 0;
                float dist = 0;

                Matrix4x4 m = new Matrix4x4(
                    V1.X - V2.X, V3.X - V2.X, -(float)ray.Direction.X, 0,
                    V1.Y - V2.Y, V3.Y - V2.Y, -(float)ray.Direction.Y, 0,
                    V1.Z - V2.Z, V3.Z - V2.Z, -(float)ray.Direction.Z, 0,
                    0, 0, 0, 1
                    );

                if (m.IsSingular)
                {
                    info = null;
                    return false;
                }

                Matrix4x4 inverse = m.Inverse;

                Vector4 res = GMath.mul(new Vector4(ray.Position.X - V2.X, ray.Position.Y - V2.Y, ray.Position.Z - V2.Z, 1), inverse);

                U = res.X;
                V = res.Y;
                dist = res.Z;

                info = new IntersectInfo(U, V, dist, new Maths.Triangle(this.V1, this.V2, this.V3));

                return true;
            }

            public double Area
            {
                get
                {
                    double A = GMath.length(V2 - V1);
                    double B = GMath.length(V3 - V1);
                    double C = GMath.length(V3 - V2);
                    return A * System.Math.Sqrt(C * C - System.Math.Pow((A * A - B * B + C * C) / (2 * A), 2)) / 2;
                }
            }

            public double Angle(Ray ray)
            {
                return Math.Acos(GMath.dot(GMath.normalize(ray.Direction), this.Normal));
            }

            public bool Intersect(Ray ray)
            {
                bool first;
                return ((first = Inside(ray, this.V1, this.V2)) == Inside(ray, this.V2, this.V3) && (first == Inside(ray, this.V3, this.V1)));
            }
        }

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

        private static IEnumerable<PositionNormalData> UnrollTriangleFan(PositionNormalData[] vertexes, int[] indexes)
        {
            if (indexes == null)
                for (int i = 2; i < vertexes.Length; i++)
                {
                    yield return vertexes[0];
                    yield return vertexes[i - 1];
                    yield return vertexes[i];
                }
            else
                for (int i = 2; i < indexes.Length; i++)
                {
                    yield return vertexes[indexes[0]];
                    yield return vertexes[indexes[i - 1]];
                    yield return vertexes[indexes[i]];
                }
        }

        private static IEnumerable<PositionNormalData> UnrollTriangleStrip(PositionNormalData[] vertexes, int[] indexes)
        {
            if (indexes == null)
                for (int i = 2; i < vertexes.Length; i++)
                {
                    yield return vertexes[i - 2];
                    yield return vertexes[i - 1];
                    yield return vertexes[i];
                }
            else
                for (int i = 2; i < indexes.Length; i++)
                {
                    yield return vertexes[indexes[i - 2]];
                    yield return vertexes[indexes[i - 1]];
                    yield return vertexes[indexes[i]];
                }
        }

        private static IEnumerable<PositionNormalData> UnrollTriangleList(PositionNormalData[] vertexes, int[] indexes)
        {
            if (indexes == null)
            {
                PositionNormalData[] positions = new PositionNormalData[vertexes.Length];
                for (int i = 0; i < vertexes.Length; i++)
                    positions[i] = vertexes[i];
                return positions;
            }
            else
            {
                PositionNormalData[] positions = new PositionNormalData[indexes.Length];
                for (int i = 0; i < indexes.Length; i++)
                    positions[i] = vertexes[indexes[i]];
                return positions;
            }
        }

        private static Triangle[] UnrollTriangles(PositionNormalData[] vertexes, int[] indexes, BasicPrimitiveType primitiveType)
        {
            PositionNormalData[] positions = null;
            switch (primitiveType)
            {
                case BasicPrimitiveType.Triangle_Fan:
                    positions = UnrollTriangleFan(vertexes, indexes).ToArray();
                    break;
                case BasicPrimitiveType.Triangle_Strip:
                    positions = UnrollTriangleStrip(vertexes, indexes).ToArray();
                    break;
                case BasicPrimitiveType.Triangles:
                    positions = UnrollTriangleList(vertexes, indexes).ToArray();
                    break;
            }

            Triangle[] triangles = new Triangle[positions.Length / 3];
            for (int i = 0; i < triangles.Length; i++)
                triangles[i] = new Triangle(positions[i * 3 + 0].Position, positions[i * 3 + 1].Position, positions[i * 3 + 2].Position, positions[i * 3 + 0].Normal, positions[i * 3 + 1].Normal, positions[i * 3 + 2].Normal);

            return triangles;
        }

        long lastVertexVersion;
        PositionNormalData[] positions;

        long lastIndexVersion;
        int[] indexes;

        internal Triangle[] GetTriangles()
        {
            // TODO: Fix this to use indexes between start index and start index plus count.
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
                        positions = (VertexBuffer.Clone<VertexBuffer, PositionNormalData>()).DirectData as PositionNormalData[];
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

        #region Creation

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

        public static Basic LineLoop<FVF>(params FVF[] buffer) where FVF : struct
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

        #endregion

        public IntersectInfo[] Intersect(Ray ray)
        {
            return GetIntersections(ray);
        }

        VertexBuffer IVertexedGraphicPrimitive.VertexBuffer
        {
            get { return VertexBuffer; }
        }

        public IVertexedGraphicPrimitive Transform(Matrix4x4 transform)
        {
            return new Basic(Type, VertexBuffer.Clone(transform), Indexes.Clone(), StartIndex, Count);
        }

        public IVertexedGraphicPrimitive Transform<FVFIn, FVFOut>(Func<FVFIn, FVFOut> transform)
            where FVFIn : struct
            where FVFOut : struct
        {
            return new Basic(Type, VertexBuffer.Clone(transform), Indexes.Clone(), StartIndex, Count);
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
