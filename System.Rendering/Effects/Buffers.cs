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

namespace System.Rendering
{
    public sealed class Buffers
    {
        private Buffers() { }

        public static Effect<FrameBufferState> FrameMask(ColorMask mask)
        {
            return new Effect<FrameBufferState>(new FrameBufferState(new Vector4(), mask).DoNotClear ());
        }

        public static Effect<FrameBufferState> Clear(float red, float green, float blue, float alpha)
        {
            return Clear(new Vector4(red, green, blue, alpha));
        }

        public static Effect<FrameBufferState> Clear(Vector4 backColor)
        {
            return new Effect<FrameBufferState>(new FrameBufferState(backColor, ColorMask.RGBA));
        }

        public static Effect<DepthBufferState> ClearDepth(float depth)
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
