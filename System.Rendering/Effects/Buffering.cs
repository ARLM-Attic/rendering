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
using System.Text;
using System.Rendering.RenderStates;
using System.Maths;

namespace System.Rendering.Effects
{
    public sealed class Buffering
    {
        private Buffering() { }

        public static Effect<FrameBufferState> Clear(Vector4 backColor)
        {
            return new Effect<FrameBufferState>(new FrameBufferState(backColor, ColorMask.RGBA));
        }

        public static Effect<DepthBufferState> ClearDepth(double depth)
        {
            return new Effect<DepthBufferState>(new DepthBufferState(depth, true, true, Compare.LessEqual));
        }

        public static Effect<DepthBufferState> ClearDepth()
        {
            return ClearDepth(1.0f);
        }

        public static Effect<StencilBufferState> ClearStencil()
        {
            return new Effect<StencilBufferState>(new StencilBufferState(true, 0, 0, StencilOp.Keep, StencilOp.Keep, StencilOp.Keep, 0, Compare.Always));
        }
    }
}
