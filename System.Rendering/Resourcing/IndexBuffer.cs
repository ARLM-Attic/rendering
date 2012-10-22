using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace System.Rendering
{
    /// <summary>
    /// Allows indexing vertexes for primitive drawing.
    /// </summary>
    public class IndexBuffer : GraphicResource, IEnumerable<int>
    {
        protected IndexBuffer(IndexBuffer.GraphicResourceStrategy strategy)
            : base(strategy)
        {
        }

        public IEnumerator<int> GetEnumerator()
        {
            IndexBuffer toIterate = (this.InnerElementType == typeof(int) && this.Location == Rendering.Location.User) ?
                this : this.Clone<IndexBuffer, int>();

            return ((IEnumerable<int>)toIterate.DirectData).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static implicit operator IndexBuffer(uint[] indexes)
        {
            return IndexBuffer.Create<IndexBuffer>(indexes, null);
        }
        public static implicit operator IndexBuffer(ushort[] indexes)
        {
            return IndexBuffer.Create<IndexBuffer>(indexes, null);
        }
        public static implicit operator IndexBuffer(int[] indexes)
        {
            return IndexBuffer.Create<IndexBuffer>(indexes, null);
        }
        public static implicit operator IndexBuffer(short[] indexes)
        {
            return IndexBuffer.Create<IndexBuffer>(indexes, null);
        }

        public override Resourcing.IAllocateable Clone(IRenderDevice render)
        {
            if (render == null) // User memory clone.
                return this.Clone<IndexBuffer>();

            return render.ResourcesManager.Load<IndexBuffer>(this.GetData());
        }
    }
}
