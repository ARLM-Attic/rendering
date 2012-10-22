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
        public Mesh Transform(Matrix4x4 transform)
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

        protected internal Mesh(IMeshManager manager)
        {
            this.manager = manager;
        }

        #endregion

        /// <summary>
        /// Transform the current mesh vertexes by a specific VertexProcessing.
        /// </summary>
        /// <param name="vertexProcess">A method for transform each vertex</param>
        /// <returns>A Mesh object with the transformed mesh</returns>
        public Mesh Transform<FVF, FVFOut>(Func<FVF, FVFOut> function)
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
    }

    class DefaultMeshManager : IMeshManager
    {
        private VertexBuffer vertexes;
        private IndexBuffer indices;
        private Box box;

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

        /// <summary>
        /// Computes the normals of vertexes automatically considering the normals of triangles each vertex correspond.
        /// </summary>
        public void ComputeNormals()
        {
            VertexBuffer toModify = Vertexes.Clone<VertexBuffer, PositionNormalData>();

            Dictionary<Vector3, Vector3> normalsForPositions = new Dictionary<Vector3, Vector3>();

            Action<Vector3, Vector3> AgregateNormal = (p, n) =>
            {
                if (!normalsForPositions.ContainsKey(p))
                    normalsForPositions.Add(p, new Vector3(0, 0, 0));

                normalsForPositions[p] += n;
            };

            foreach (var triangle in GetTriangles(toModify, Indices))
            {
                var normal = triangle.Normal;

                AgregateNormal(triangle.V1, normal);
                AgregateNormal(triangle.V2, normal);
                AgregateNormal(triangle.V3, normal);
            }

            foreach (var p in new List<Vector3>(normalsForPositions.Keys))
                normalsForPositions[p] = GMath.normalize(normalsForPositions[p]);

            Vertexes.Process<PositionNormalData>(v => new PositionNormalData() { Position = v.Position, Normal = normalsForPositions[v.Position] });
        }

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

        #region IModel Members

        public void InternalDraw(ITessellator tessellator)
        {
            tessellator.Draw(Basic.Create(BasicPrimitiveType.Triangles, Vertexes, Indices));
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

        public void WeldVertexes(float epsilon)
        {

        }

        public IMeshManager Tessellated(float segments)
        {
            return new DefaultMeshManager(this.Vertexes.Clone(), this.Indices.Clone());
        }
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
    }


    public interface IMeshManager
    {
        VertexBuffer Vertexes { get; }

        IndexBuffer Indices { get; }

        IEnumerable<IntersectInfo> Intersect(Ray ray);

        void ComputeNormals();

        void WeldVertexes(float epsilon);

        IMeshManager Tessellated(float segments);

        void InternalDraw(ITessellator tessellator);

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
