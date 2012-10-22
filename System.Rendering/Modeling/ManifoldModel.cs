#define FLOAT

#if FLOAT
using FLOATINGTYPE = System.Single;
#endif
#if DOUBLE
using FLOATINGTYPE = System.Double;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VERTEX = System.Rendering.PositionNormalCoordinatesData;
using System.Rendering.Resourcing;
using System.Maths;

namespace System.Rendering.Modeling
{
    public class ManifoldModel : AllocateableBase, IModel
    {
        Basic primitive;
        IManifold manifold;
        int[] slices;

        public static ManifoldModel Surface(Func<float, float, Vector3> surface)
        {
            return Surface(surface, 32, 32, true);
        }
        public static ManifoldModel Surface(Func<float, float, Vector3> surface, int slices, int stacks, bool generateNormals)
        {
            var mm = new ManifoldModel(Manifold.Surface(surface), slices, stacks);
            if (generateNormals)
                mm.ComputeNormals();
            return mm;
        }

        protected ManifoldModel()
        {
        }

        private ManifoldModel(Basic primitive, int[] slices)
        {
            this.primitive = primitive;
            this.slices = slices;
        }

        private ManifoldModel(IManifold manifold, params int[] slices)
        {
            var vertexesArray = GetVertexes(manifold, slices);
            uint[] indexes = GetIndexes(manifold, slices);
            this.slices = slices;
            this.manifold = manifold;
            switch (manifold.Dimension)
            {
                case 0: primitive = Basic.Points(vertexesArray);
                    break;
                case 1: primitive = Basic.LineStrip(vertexesArray);
                    break;
                case 2:
                    WeldVertexes(vertexesArray, 0.0001f);
                    primitive = Basic.Create(BasicPrimitiveType.Triangles, (VertexBuffer)vertexesArray, (IndexBuffer)indexes);
                    //ComputeNormals();
                    break;
                case 3: primitive = Basic.Create(BasicPrimitiveType.Box, (VertexBuffer)vertexesArray, (IndexBuffer)indexes);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public int Slices
        {
            get
            {
                return slices[slices.Length - 1];
            }
        }

        public int Stacks
        {
            get
            {
                if (slices.Length < 2)
                    throw new InvalidOperationException();
                return slices[slices.Length - 2];
            }
        }

        public int Planes
        {
            get
            {
                if (slices.Length < 3)
                    throw new InvalidOperationException();
                return slices[slices.Length - 3];
            }
        }

        private void WeldVertexes(Array vertexes, float epsilon)
        {
            VERTEX[] tab = vertexes as VERTEX[];
            for (int i = 0; i <= slices[0]; i++)
            {
                int index1 = i * (slices[1] + 1);
                int index2 = i * (slices[1] + 1) + slices[1];
                if (GMath.length(tab[index1].Position - tab[index2].Position) <= epsilon)
                    tab[index2].Position = tab[index1].Position;
            }

            for (int j = 0; j <= slices[1]; j++)
            {
                int index1 = j;
                int index2 = (slices[1] + 1) * (slices[0]) + j;
                if (GMath.length(tab[index1].Position - tab[index2].Position) <= epsilon)
                    tab[index2].Position = tab[index1].Position;
            }
        }

        private void ComputeNormals()
        {
            PositionNormalData[] toModify = primitive.VertexBuffer.GetData<PositionNormalData>() as PositionNormalData[];
            uint[] indexes = primitive.Indexes.GetData<uint>() as uint[];

            for (int i = 0; i < indexes.Length / 3; i++)
            {
                Triangle t = new Triangle(toModify[indexes[i * 3 + 0]].Position, toModify[indexes[i * 3 + 2]].Position, toModify[indexes[i * 3 + 1]].Position);
                var v = t.Normal;
                toModify[indexes[i * 3 + 0]].Normal += v;
                toModify[indexes[i * 3 + 1]].Normal += v;
                toModify[indexes[i * 3 + 2]].Normal += v;
            }

            for (int i = 0; i < toModify.Length; i++)
                toModify[i].Normal = GMath.normalize(toModify[i].Normal);

            primitive.VertexBuffer.Update(toModify);
        }

        private void ComputeNormals2()
        {
            PositionData[] toModify = primitive.VertexBuffer.GetData<PositionData>() as PositionData[];

            Dictionary<Vector3, Vector3> normalsForPositions = new Dictionary<Vector3, Vector3>();

            Action<Vector3, Vector3> AgregateNormal = (p, n) =>
            {
                if (!normalsForPositions.ContainsKey(p))
                    normalsForPositions.Add(p, new Vector3(0, 0, 0));

                normalsForPositions[p] += n;
            };

            foreach (var triangle in GetTriangles(toModify))
            {
                var normal = triangle.Normal;

                AgregateNormal(triangle.V1, normal);
                AgregateNormal(triangle.V2, normal);
                AgregateNormal(triangle.V3, normal);
            }

            foreach (var p in new List<Vector3>(normalsForPositions.Keys))
                normalsForPositions[p] = GMath.normalize(normalsForPositions[p]);

            // this is for handle some problems when copying the buffer onto toModify variable
            //if(normalsForPositions.Count == primitive.VertexBuffer.Length)
              primitive.VertexBuffer.Process<PositionNormalData>(v => new PositionNormalData() { Position = v.Position, Normal = -1 * normalsForPositions[v.Position] });
        }

        private IEnumerable<Triangle> GetTriangles(PositionData[] positions)
        {
            int[] indexes = primitive.Indexes.ToArray();

            for (int i = 0; i < indexes.Length / 3; i++)
                yield return new Triangle(
                    positions[indexes[i * 3 + 0]].Position,
                    positions[indexes[i * 3 + 1]].Position,
                    positions[indexes[i * 3 + 2]].Position);
        }

        private static uint[] GetIndexes(IManifold manifold, int[] slices)
        {
            switch (slices.Length)
            {
                case 0: return null;
                case 1: return null;
                case 2: return GetQuadricIndexes(slices[0], slices[1]);
                case 3: return GetBoxicIndexes(slices[0], slices[1], slices[2]);
                default: throw new NotSupportedException();
            }
        }

        private static uint[] GetQuadricIndexes(int stacks, int slices)
        {
            uint[] con = new uint[6 * stacks * slices];
            int cont = 0;
            for (int i = 0; i < stacks; i++)
                for (int j = 0; j < slices; j++)
                    if ((i + j) % 2 == 0)
                    {
                        con[cont++] = (uint)(i * (slices + 1) + j);
                        con[cont++] = (uint)((i + 1) * (slices + 1) + j);
                        con[cont++] = (uint)((i + 1) * (slices + 1) + (j + 1));

                        con[cont++] = (uint)(i * (slices + 1) + j);
                        con[cont++] = (uint)((i + 1) * (slices + 1) + (j + 1));
                        con[cont++] = (uint)(i * (slices + 1) + (j + 1));
                    }
                    else
                    {
                        con[cont++] = (uint)(i * (slices + 1) + j);
                        con[cont++] = (uint)((i + 1) * (slices + 1) + j);
                        con[cont++] = (uint)((i) * (slices + 1) + (j + 1));

                        con[cont++] = (uint)((i + 1) * (slices + 1) + j);
                        con[cont++] = (uint)((i + 1) * (slices + 1) + (j + 1));
                        con[cont++] = (uint)(i * (slices + 1) + (j + 1));
                    }
            return con;
        }

        private static uint[] GetBoxicIndexes(int slices, int stacks, int planes)
        {
            throw new NotImplementedException();
        }

        private static VERTEX[] GetVertexes(IManifold manifold, int[] slices)
        {
            VERTEX[] vertexes = new VERTEX[slices.Aggregate(1, (l, i) => l * (i + 1))];

            switch (manifold.Dimension)
            {
                case 0: vertexes[0] = VERTEX.Create(((Manifold0)manifold).Position, Vectors.O, new Vector2(0, 0));
                    break;
                case 1: for (int i = 0; i <= slices[0]; i++)
                    {
                        float u = i / (float)slices[0];
                        vertexes[i] = VERTEX.Create(((Manifold1)manifold)[u].Position, Vectors.O, new Vector2(u, 0));
                    }
                    break;
                case 2:
                    for (int i = 0; i <= slices[0]; i++)
                        for (int j = 0; j <= slices[1]; j++)
                        {
                            float u = i / (float)slices[0];
                            float v = j / (float)slices[1];
                            vertexes[i * (slices[1] + 1) + j] = VERTEX.Create(((Manifold2)manifold)[u, v].Position, Vectors.O, new Vector2(u, v));
                            //vertexes[i * (slices[1] + 1) + j] = new VERTEX(new Vector3<float>(u, v, 0), Vectors.O, u, v);
                        }
                    break;
                default: throw new NotImplementedException();
            }

            return vertexes;
        }

        public ManifoldModel(Manifold0 manifold)
            : this((IManifold)manifold)
        {
        }

        public ManifoldModel(Manifold1 manifold, int slices)
            : this((IManifold)manifold, slices)
        {
        }

        public ManifoldModel(Manifold2 manifold, int slices, int stacks)
            : this((IManifold)manifold, slices, stacks)
        {
        }

        public ManifoldModel(Manifold3 manifold, int slices, int stacks, int planes)
            : this((IManifold)manifold, slices, stacks, planes)
        {
        }

        public void Tesselate(ITessellator tessellator)
        {
            tessellator.Draw(this.primitive);
        }

        protected override Location OnClone(AllocateableBase toFill, IRenderDevice render)
        {
            ManifoldModel filling = toFill as ManifoldModel;

            filling.slices = this.slices;
            VertexBuffer vbAllocated = (VertexBuffer)primitive.VertexBuffer.Clone(render);
            IndexBuffer ibAllocated = primitive.Indexes == null ? null : (IndexBuffer)primitive.Indexes.Clone(render);
            filling.primitive = Basic.Create(primitive.Type, vbAllocated, ibAllocated);

            if (filling.primitive.Indexes == null)
                return filling.primitive.VertexBuffer.Location;

            if (filling.primitive.VertexBuffer.Location == Rendering.Location.Device && filling.primitive.Indexes.Location == Rendering.Location.Device)
                return Rendering.Location.Device;
            if (filling.primitive.VertexBuffer.Location == Rendering.Location.User || filling.primitive.Indexes.Location == Rendering.Location.User)
                return Rendering.Location.User;
            return Rendering.Location.Render;
        }

        protected override void OnDispose()
        {
            primitive.VertexBuffer.Dispose();
            if (primitive.Indexes != null)
                primitive.Indexes.Dispose();
        }

        public bool IsSupported(IRenderDevice render)
        {
            return render.TessellatorInfo.IsSupported<Basic>();
        }
    }
}
