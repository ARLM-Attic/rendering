using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Resourcing;

namespace System.Rendering.Modeling
{
    public class SingleModel<GP> : AllocateableBase, IModel where GP : struct, IGraphicPrimitive
    {
        public GP Primitive { get; private set; }

        public SingleModel(GP primitive) { this.Primitive = primitive; }

        protected override Location OnClone(AllocateableBase toFill, IRenderDevice render)
        {
            if (Primitive is IAllocateable)
            {
                var clone = (GP)((IAllocateable)Primitive).Allocate(render);
                ((SingleModel<GP>)toFill).Primitive = clone;

                return ((IAllocateable)clone).Location;
            }
            else
            {
                ((SingleModel<GP>)toFill).Primitive = this.Primitive;
                return Location.User;
            }
        }

        protected override void OnDispose()
        {
            if (Primitive is IAllocateable)
                ((IAllocateable)Primitive).Dispose();
        }

        public bool IsSupported(IRenderDevice device)
        {
            return device.TessellatorInfo.IsSupported<GP>();
        }

        public void Tesselate(ITessellator tessellator)
        {
            tessellator.Draw(Primitive);
        }

        public IModel Transformed(Maths.Matrix4x4 transform)
        {
            var newone = MemberwiseClone () as SingleModel<GP>;

            if (Primitive is IVertexedGraphicPrimitive)
                newone.Primitive = (GP)((IVertexedGraphicPrimitive)Primitive).Transform(transform);

            return newone;
        }

        public IModel Transformed<FVFIn, FVFOut>(Func<FVFIn, FVFOut> transform)
            where FVFIn : struct
            where FVFOut : struct
        {
            var newone = MemberwiseClone() as SingleModel<GP>;

            if (Primitive is IVertexedGraphicPrimitive)
                newone.Primitive = (GP)((IVertexedGraphicPrimitive)Primitive).Transform(transform);

            return newone;
        }
    }
}
