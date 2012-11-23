#define FLOAT

#if DECIMAL
using FLOATINGTYPE = System.Decimal;
#endif
#if DOUBLE
using FLOATINGTYPE = System.Double;
#endif
#if FLOAT
using FLOATINGTYPE = System.Single;
#endif

using System;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Rendering.Resourcing;
using System.Maths;
using System.Rendering.Services;

namespace System.Rendering.Modeling
{
    /// <summary>
    /// Implementation of IMesh interface that can be used for representing models like spheres, cylinders, cones, cubes.
    /// </summary>
    public class Mesh : AllocateableBase, IModel
    {
        IMeshManager manager;

        /// <summary>
        /// Transform the current mesh vertexes by a specific Matrices.
        /// </summary>
        /// <param name="transform">A Matrix4x4 structure used for transforming the vertexes</param>
        /// <returns>A Mesh object representing the transformed mesh</returns>
        public Mesh Transformed(Matrix4x4 transform)
        {
            VertexBuffer newVB = Vertices.Clone(transform);

            return new Mesh(newVB, Indices.Clone());
        }

        #region Constructors

        /// <summary>
        /// Initialize the mesh using a set of vertexes and a secuence of indexes.
        /// </summary>
        /// <param name="vertexes"></param>
        /// <param name="conections"></param>
        protected Mesh(VertexBuffer vertexes, IndexBuffer indices)
        {
            this.manager = new DefaultMeshManager(vertexes, indices);
        }

        protected internal Mesh(IMeshManager manager) : base (manager.Vertexes.Render)
        {
            this.manager = manager;
        }

        #endregion

        /// <summary>
        /// Transform the current mesh vertexes by a specific VertexProcessing.
        /// </summary>
        /// <param name="vertexProcess">A method for transform each vertex</param>
        /// <returns>A Mesh object with the transformed mesh</returns>
        public Mesh Transformed<FVF, FVFOut>(Func<FVF, FVFOut> function)
            where FVF : struct
            where FVFOut : struct
        {
            VertexBuffer vb = this.Vertices.Clone<FVF, FVFOut>(function);
            return new Mesh(vb, Indices.Clone());
        }

        /// <summary>
        /// Computes the normals of vertexes automatically considering the normals of triangles each vertex correspond.
        /// </summary>
        public void ComputeNormals()
        {
            manager.ComputeNormals();
        }

        public void WeldVertexes(float epsilon)
        {
            manager.WeldVertexes(epsilon);
        }

        private IEnumerable<Triangle> GetTriangles(VertexBuffer vertexBufferPositionNormal, IndexBuffer indexBuffer)
        {
            IEnumerator<PositionNormalData> e = Vertices.Indexed<PositionNormalData>(Indices).GetEnumerator();
            while (e.MoveNext())
            {
                PositionNormalData vertex1 = e.Current;
                if (e.MoveNext())
                {
                    PositionNormalData vertex2 = e.Current;
                    if (e.MoveNext())
                    {
                        PositionNormalData vertex3 = e.Current;

                        yield return new Triangle(vertex1.Position, vertex2.Position, vertex3.Position);
                    }
                    else
                        yield break;
                }
                else
                    yield break;
            }
        }

        public IEnumerable<Triangle> Triangles
        {
            get
            {
                return GetTriangles(Vertices.Clone<VertexBuffer, PositionNormalData>(), Indices);
            }
        }

        /// <summary>
        /// Intersects a ray with the mesh and retrieves the set of intersections.
        /// </summary>
        /// <param name="ray">Ray to check intersection</param>
        /// <returns>Set of intersections resutl of raycast</returns>
        public IEnumerable<IntersectInfo> Intersect(Ray ray)
        {
            int count = 0;
            foreach (var t in Triangles)
            {
                IntersectInfo info;
                if (t.Intersect(ray, out info))
                {
                    if (info.Distance >= 0)
                    {
                        info.TriangleIndex = count;
                        yield return info;
                    }
                }
                count++;
            }
        }

        #region IModel Members

        protected virtual void OnTesselate(ITessellator tessellator)
        {
            tessellator.Draw(Basic.Create(BasicPrimitiveType.Triangles, Vertices, Indices));
        }

        void IModel.Tesselate(ITessellator tessellator)
        {
            OnTesselate(tessellator);
        }

        public Mesh Tessellated(float segments)
        {
            return new Mesh(manager.Tessellated(segments));
        }

        #endregion

        public VertexBuffer Vertices
        {
            get { return manager.Vertexes; }
        }

        public IndexBuffer Indices
        {
            get
            {
                return manager.Indices;
            }
        }

        protected override Location OnClone(AllocateableBase toFill, IRenderDevice render)
        {
            Mesh filling = toFill as Mesh;

            if (render.Services.Support<MeshService>())
            {
                var manager = render.Services.Get<MeshService>().Factory.Create(this.Vertices, this.Indices);
                filling.manager = manager;
                return Location.Device;
            }
            else
            {
                DefaultMeshManager defaultManager = new DefaultMeshManager(this.Vertices.Clone<VertexBuffer>(render), this.Indices.Clone<IndexBuffer>(render));
                filling.manager = defaultManager;
                if (filling.Vertices.Location == Rendering.Location.Device && filling.Indices.Location == Rendering.Location.Device)
                    return Rendering.Location.Render;

                return Rendering.Location.User;
            }
        }

        protected override void OnDispose()
        {
            Vertices.Dispose();
            Indices.Dispose();
        }

        public bool IsSupported(IRenderDevice render)
        {
            return render.TessellatorInfo.IsSupported<Basic>();
        }

        IModel IModel.Transformed(Matrix4x4 transform)
        {
            return Transformed(transform);
        }

        IModel IModel.Transformed<FVFIn, FVFOut>(Func<FVFIn, FVFOut> transform)
        {
            return Transformed(transform);
        }
    }

    class DefaultMeshManager : IMeshManager
    {
        private VertexBuffer vertexes;
        private IndexBuffer indices;

        #region Constructors

        /// <summary>
        /// Initialize the mesh using a set of vertexes and a secuence of indexes.
        /// </summary>
        /// <param name="vertexes"></param>
        /// <param name="conections"></param>
        public DefaultMeshManager(VertexBuffer vertexes, IndexBuffer indices)
        {
            this.vertexes = vertexes;
            this.indices = indices;
        }

        #endregion

        #region Compute Normals

        /// <summary>
        /// Computes the normals of vertexes automatically considering the normals of triangles each vertex correspond.
        /// </summary>
        public void ComputeNormals()
        {
            PositionNormalData[] positions = Vertexes.GetData<PositionNormalData>();
            int[] indexes = Indices.GetData<int>();

            Vector3[] normals = new Vector3[positions.Length];

            bool[] discard = new bool[positions.Length];
            int[] replaced = new int[positions.Length];

            Dictionary<Vector3, int> cluster = new Dictionary<Vector3, int>(new Vector3EpsilonEqualityComparer());

            for (int i = 0; i < positions.Length; i++)
                if (!cluster.ContainsKey(positions[i].Position))
                {
                    replaced[i] = i;
                    cluster.Add(positions[i].Position, i);
                }
                else
                {
                    replaced[i] = cluster[positions[i].Position];
                    discard[i] = true;
                }

            for (int i = 0; i < indexes.Length / 3; i++)
            {
                Triangle t = new Triangle(positions[indexes[i * 3 + 0]].Position, positions[indexes[i * 3 + 2]].Position, positions[indexes[i * 3 + 1]].Position);
                var v = t.Normal;
                normals[replaced[indexes[i * 3 + 0]]] += v;
                normals[replaced[indexes[i * 3 + 1]]] += v;
                normals[replaced[indexes[i * 3 + 2]]] += v;
            }

            for (int i = 0; i < positions.Length; i++)
                positions[i].Normal = GMath.normalize(normals[replaced[i]]);

            Vertexes.Update(positions);
        }

        public void ComputeTangents()
        {
        }

        #endregion

        #region Intersect

        private IEnumerable<Triangle> GetTriangles(VertexBuffer vertexBufferPositionNormal, IndexBuffer indexBuffer)
        {
            IEnumerator<PositionNormalData> e = Vertexes.Indexed<PositionNormalData>(Indices).GetEnumerator();
            while (e.MoveNext())
            {
                PositionNormalData vertex1 = e.Current;
                if (e.MoveNext())
                {
                    PositionNormalData vertex2 = e.Current;
                    if (e.MoveNext())
                    {
                        PositionNormalData vertex3 = e.Current;

                        yield return new Triangle(vertex1.Position, vertex2.Position, vertex3.Position);
                    }
                    else
                        yield break;
                }
                else
                    yield break;
            }
        }

        private IEnumerable<Triangle> Triangles
        {
            get
            {
                return GetTriangles(Vertexes.Clone<VertexBuffer, PositionNormalData>(), Indices);
            }
        }

        /// <summary>
        /// Intersects a ray with the mesh and retrieves the set of intersections.
        /// </summary>
        /// <param name="ray">Ray to check intersection</param>
        /// <returns>Set of intersections resutl of raycast</returns>
        public IEnumerable<IntersectInfo> Intersect(Ray ray)
        {
            int count = 0;
            foreach (var t in Triangles)
            {
                IntersectInfo info;
                if (t.Intersect(ray, out info))
                {
                    if (info.Distance >= 0)
                    {
                        info.TriangleIndex = count;
                        yield return info;
                    }
                }
                count++;
            }
        }

        #endregion

        public VertexBuffer Vertexes
        {
            get { return this.vertexes; }
        }

        public IndexBuffer Indices
        {
            get
            {
                return this.indices;
            }
        }

        public void Dispose()
        {
            Vertexes.Dispose();
            Indices.Dispose();
        }

        #region Weld Vertices
        class Vector3EpsilonEqualityComparer : IEqualityComparer<Vector3>
        {
            Vector3 Normalize(Vector3 x)
            {
                return new Vector3((float)Math.Round(x.X, 6), (float)Math.Round(x.Y, 6), (float)Math.Round(x.Z, 6));
            }

            public bool Equals(Vector3 x, Vector3 y)
            {
                return Normalize(x).Equals(Normalize(y));
            }

            public int GetHashCode(Vector3 obj)
            {
                return Normalize(obj).GetHashCode();
            }
        }

        public void WeldVertexes(float epsilon)
        {
            PositionData[] positions = Vertexes.GetData<PositionData>();
            int[] indexes = Indices.GetData<int>();
            bool[] discard = new bool[positions.Length];
            int[] replaced = new int[positions.Length];
            int newVertexes = 0;

            if (epsilon < 0.000001f)
            {
                Dictionary<Vector3, int> cluster = new Dictionary<Vector3, int>(new Vector3EpsilonEqualityComparer());

                for (int i=0; i<positions.Length; i++)
                    if (!cluster.ContainsKey(positions[i].Position))
                    {
                        replaced[i] = newVertexes;
                        cluster.Add(positions[i].Position, newVertexes);
                        newVertexes ++;
                    }
                    else
                    {
                        replaced[i] = cluster[positions[i].Position];
                        discard[i] = true;
                    }
            }
            else
            {
                Dictionary<Tuple<int, int, int>, List<Tuple<Vector3, int>>> cluster = new Dictionary<Tuple<int, int, int>, List<Tuple<Vector3, int>>>();

                float sqrEpsilon = epsilon * epsilon;
                
                for (int i =0; i<positions.Length; i++)
                {
                    var p = positions[i];
                    replaced[i] = -1;

                    int xc = (int)Math.Round(p.Position.X / epsilon);
                    int yc = (int)Math.Round(p.Position.Y / epsilon);
                    int zc = (int)Math.Round(p.Position.Z / epsilon);

                    Tuple<int,int,int> cell = Tuple.Create(xc, yc, zc);

                    /// Check in 9 (adyacents) cells
                    for (int dx = -1; dx <= 1; dx++)
                        for (int dy = -1; dy <= 1; dy++)
                            for (int dz = -1; dz <= 1; dz++)
                            {
                                var adjCell = Tuple.Create (xc+dx, yc+dy, zc+dz);
                                if (cluster.ContainsKey(adjCell))
                                {
                                    List<Tuple<Vector3, int>> l = cluster[adjCell];

                                    foreach (var t in l)
                                    {
                                        var vec = t.Item1 - p.Position;
                                        if (GMath.dot(vec, vec) <= sqrEpsilon)
                                            replaced[i] = t.Item2;
                                    }
                                }
                            }

                    if (replaced[i] == -1)
                    {
                        replaced[i] = newVertexes;

                        if (!cluster.ContainsKey(cell))
                            cluster.Add(cell, new List<Tuple<Vector3, int>>());

                        cluster[cell].Add(Tuple.Create(p.Position, newVertexes));

                        newVertexes++;
                    }
                    else
                        discard[i] = true;
                }
            }

            #region Discard Vertexes

            var newVertexesArray = Array.CreateInstance(Vertexes.InnerElementType, newVertexes);

            Array srcArray = Vertexes.GetData();

            int index = 0;
            int j = 0;

            foreach (var v in srcArray)
            {
                if (!discard[index])
                {
                    newVertexesArray.SetValue(v, j);
                    j++;
                }
                index++;
            }

            #endregion

            #region Change Indexes

            for (int i = 0; i < indexes.Length; i++)
                indexes[i] = replaced[indexes[i]];

            #endregion

            var render = Vertexes.Render;

            Vertexes.Dispose();

            vertexes = ((VertexBuffer)newVertexesArray).Allocate(render);

            indices.Update(indexes);
        }
        #endregion

        #region Tessellated

        public IMeshManager Tessellated(float segments)
        {
            // TODO: Implement here a software tessellation of the mesh.
            return new DefaultMeshManager(this.Vertexes.Clone(), this.Indices.Clone());
        }

        #endregion
    }

    public class Mesh<FVF> : Mesh where FVF : struct
    {
        public Mesh(FVF[] vertexes, uint[] indexes)
            : base((VertexBuffer)vertexes, (IndexBuffer)indexes)
        {
        }
        public Mesh(FVF[] vertexes, ushort[] indexes)
            : base((VertexBuffer)vertexes, (IndexBuffer)indexes)
        {
        }

        public static Mesh<FVF> Empty { get { return new Mesh<FVF>(new FVF[0], new uint[0]); } }
    }

    public interface IMeshManager
    {
        VertexBuffer Vertexes { get; }

        IndexBuffer Indices { get; }

        IEnumerable<IntersectInfo> Intersect(Ray ray);

        void ComputeNormals();

        void ComputeTangents();

        void WeldVertexes(float epsilon);

        IMeshManager Tessellated(float segments);

        void Dispose();
    }

    /// <summary>
    /// Tool class used for performance in mesh intersection
    /// </summary>
	public sealed class Box
	{
        Triangle[] t;

        public Box(Vector3 maxVector, Vector3 minVector)
        {
            Vector3[] vecs = new Vector3[]{
		    Vectors.O, 
            Vectors.Right, 
            Vectors.Right+Vectors.Up, 
            Vectors.Up, 
            Vectors.O+Vectors.Front, 
            Vectors.Right+Vectors.Front, 
            Vectors.Right+Vectors.Up+Vectors.Front, 
            Vectors.Up+Vectors.Front};

            Matrix4x4 transform = GMath.mul (Matrices.Scale((Vector3)(maxVector - minVector)), Matrices.Translate((Vector3)minVector));

            for (int i = 0; i < vecs.Length; i++)
                vecs[i] = GMath.mul(new Vector4(vecs[i], 1), transform).Homogeneous;

            int[] indexes = new int[] { 
                0, 2, 1, 
                0, 3, 2, 
                0, 1, 5, 
                0, 5, 4, 
                0, 4, 7, 
                0, 7, 3, 
                6, 5, 1, 
                6, 1, 2, 
                6, 2, 3, 
                6, 3, 7, 
                6, 7, 4, 
                6, 4, 5 };

            t = new Triangle[12];
            for (int i = 0; i < t.Length; i++)
            {
                t[i] = new Triangle((Vector3)vecs[indexes[i * 3 + 0]], (Vector3)vecs[indexes[i * 3 + 1]], (Vector3)vecs[indexes[i * 3 + 2]]);
            }
        }

        public bool Intersect(Ray ray)
		{
			foreach (Triangle tr in t)
				if (tr.Intersect(ray))
                    return true;
			return false;
		}
	}
}
