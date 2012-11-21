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
    public interface IManifoldModel : IModel
    {
        IManifold Manifold { get; }

        void Invalidate();
    }

    public class PointModel : SingleModel<Basic>, IManifoldModel
    {
        public PointModel(Manifold0 manifold):base (Basic.Points (new PositionData { Position = new Vector3 () }))
        {
            this.Manifold = manifold;
            this.Invalidate();
        }

        public Manifold0 Manifold
        {
            get;
            private set;
        }

        IManifold IManifoldModel.Manifold { get { return Manifold; } }

        public void Invalidate()
        {
            PositionData[] data = Primitive.VertexBuffer.GetData<PositionData>();
            data[0].Position = Manifold.Position;
            Primitive.VertexBuffer.Update(data);
        }
    }

    public class CurveModel : SingleModel<Basic>, IManifoldModel
    {
        public CurveModel(Manifold1 manifold, int slices)
            : base(Basic.LineStrip(new PositionData[slices + 1]))
        {
            this.Manifold = manifold;
            this.Slices = slices;
            Invalidate();
        }

        public CurveModel(Manifold1 manifold)
            : this(manifold, 32)
        {
        }

        public int Slices { get; private set; }

        public Manifold1 Manifold
        {
            get;
            private set;
        }

        IManifold IManifoldModel.Manifold { get { return Manifold; } }

        public void Invalidate()
        {
            PositionData[] data = Primitive.VertexBuffer.GetData<PositionData>();
            
            for (int i = 0; i < Slices + 1; i++)
                data[i].Position = Manifold.GetPositionAt(i / (float)Slices);

            Primitive.VertexBuffer.Update(data);
        }
    }

    public class SurfaceModel : Mesh<PositionNormalCoordinatesData>, IManifoldModel
    {
        public const int PRESITION = 7;

        private static uint[] GetQuadricIndexes(int stacks, int slices)
        {
            uint[] con = new uint[6 * stacks * slices];
            int cont = 0;
            for (int i = 0; i < stacks; i++)
                for (int j = 0; j < slices; j++)
                    //if ((i + j) % 2 == 0)
                    {
                        con[cont++] = (uint)(i * (slices + 1) + j);
                        con[cont++] = (uint)((i + 1) * (slices + 1) + (j + 1));
                        con[cont++] = (uint)((i + 1) * (slices + 1) + j);

                        con[cont++] = (uint)(i * (slices + 1) + j);
                        con[cont++] = (uint)(i * (slices + 1) + (j + 1));
                        con[cont++] = (uint)((i + 1) * (slices + 1) + (j + 1));
                    }
                    //else
                    //{
                    //    con[cont++] = (uint)(i * (slices + 1) + j);
                    //    con[cont++] = (uint)((i + 1) * (slices + 1) + j);
                    //    con[cont++] = (uint)((i) * (slices + 1) + (j + 1));

                    //    con[cont++] = (uint)((i + 1) * (slices + 1) + j);
                    //    con[cont++] = (uint)((i + 1) * (slices + 1) + (j + 1));
                    //    con[cont++] = (uint)(i * (slices + 1) + (j + 1));
                    //}
            return con;
        }

        public Manifold2 Manifold
        {
            get;
            private set;
        }

        static Vector3 Normalize(Vector3 x)
        {
            return new Vector3((float)Math.Round(x.X, PRESITION), (float)Math.Round(x.Y, PRESITION), (float)Math.Round(x.Z, PRESITION));
        }

        private static VERTEX[] GetVertexes(Manifold2 manifold, int stacks, int slices)
        {
            VERTEX[] vertexes = new VERTEX[(stacks + 1) * (slices + 1)];

            float epsilon = 0.00001f;

            for (int i = 0; i <= stacks; i++)
                for (int j = 0; j <= slices; j++)
                {
                    float u = i / (float)stacks;
                    float v = j / (float)slices;

                    Vector3 position = Normalize (manifold[u, v].Position);
                    Vector3 positionPlusDx = manifold[u + epsilon, v].Position;
                    Vector3 positionPlusDy = manifold[u, v + epsilon].Position;

                    //vertexes[i * (slices + 1) + j] = VERTEX.Create(position, Vectors.Front, new Vector2(u, v));
                    vertexes[i * (slices + 1) + j] = VERTEX.Create(position, GMath.normalize(GMath.cross(positionPlusDx - position, positionPlusDy - position)), new Vector2(u, v));
                }

            return vertexes;
        }

        public bool AutoComputeNormals
        {
            get;
            set;
        }

        public int Stacks { get; private set; }

        public int Slices { get; private set; }

        public SurfaceModel(Manifold2 manifold, int slices, int stacks, bool autoComputeNormals)
            : base(new VERTEX[(stacks + 1) * (slices + 1)], GetQuadricIndexes(slices, stacks))
        {
            this.Manifold = manifold;
            this.Slices = slices;
            this.Stacks = stacks;
            this.AutoComputeNormals = autoComputeNormals;
            this.Invalidate();
        }

        public SurfaceModel(Manifold2 manifold, int slices, int stacks)
            : this(manifold, slices, stacks, true)
        {
        }

        public SurfaceModel(Manifold2 manifold)
            : this(manifold, 32, 32)
        {
        }

        IManifold IManifoldModel.Manifold
        {
            get { return Manifold; }
        }

        public void Invalidate()
        {
            var vertexes = GetVertexes(Manifold, Slices, Stacks);
            this.Vertices.Update(vertexes);
            if (AutoComputeNormals)
                this.ComputeNormals();
        }
    }
}
