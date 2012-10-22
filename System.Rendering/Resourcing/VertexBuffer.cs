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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Resourcing;
using System.Maths;

namespace System.Rendering
{
    /// <summary>
    /// Allows to have a collection of vertexes for primitive drawing.
    /// </summary>
    public class VertexBuffer : GraphicResource
    {
        protected VertexBuffer(GraphicResource.GraphicResourceStrategy strategy)
            : base(strategy)
        {
        }

        public static implicit operator VertexBuffer(Array array)
        {
            return GraphicResource.Create<VertexBuffer>(array, null);
        }

        public override Resourcing.IAllocateable Clone(IRenderDevice render)
        {
            if (render == null) // User memory clone.
                return this.Clone<VertexBuffer>();

            return render.ResourcesManager.Load<VertexBuffer>(this.GetData());
        }

        public VertexBuffer Clone<FVF, ResultFVF>(Func<FVF, ResultFVF> transform)
            where FVF : struct
            where ResultFVF : struct
        {
            Array dataSrc = this.GetData<FVF>();
            Array dataDst = Array.CreateInstance(typeof(ResultFVF), this.GetRanks());

            DataComponentExtensors.Copy<FVF, ResultFVF>(dataSrc, dataDst, e => transform(e));
            
            return VertexBuffer.Create<VertexBuffer>(dataDst, null);
        }

        public VertexBuffer Clone(Matrix4x4 transform)
        {
            Array dataTemp = this.GetData<PositionNormalData>();

            Array transformed = dataTemp.Cast<PositionNormalData>().Select(e => new PositionNormalData()
            {
                Position = (Vector3)GMath.mul(new Vector4(e.Position, 1), transform),
                Normal = (Vector3)GMath.mul(new Vector4(e.Normal, 0), transform)
            }).ToArray();

            VertexBuffer clone = this.Clone();

            clone.SetData(GraphicResourceUpdateMode.Update, transformed, null);

            return clone;
        }

        public IEnumerable<FVF> Indexed<FVF>(IndexBuffer indexBuffer) where FVF : struct
        {
            VertexBuffer toIterate = (this.InnerElementType == typeof(FVF)) ? this : this.Clone<VertexBuffer, FVF>();

            List<FVF> toStore = new List<FVF>(toIterate.Length);
            toStore.AddRange(toIterate.DirectData.Cast<FVF>());

            if (indexBuffer == null)
                return toStore;
            else
                return StoreIndexed(toStore, indexBuffer);
        }

        private IEnumerable<FVF> StoreIndexed<FVF>(List<FVF> toStore, IndexBuffer indexBuffer) where FVF:struct
        {
            foreach (var index in indexBuffer)
                yield return toStore[index];
        }

        public void Process<FVF>(Func<FVF, FVF> process) where FVF : struct
        {
            var clone = this.Clone<FVF, FVF>(v => process(v));
            this.SetData(GraphicResourceUpdateMode.Update, clone.DirectData, null);
        }
    }
}
