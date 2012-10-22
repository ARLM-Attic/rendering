using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Effects.Shaders;
using System.Rendering.RenderStates;

namespace System.Rendering.Effects
{
    public class VertexShaderEffect <VIn, VOut> : BlendingEffect<VertexShaderState>
        where VIn : struct
        where VOut : struct
    {
        VertexShaderEffect() :base() { }

        VertexShaderEffect(VertexShaderState state)
            : base(state)
        {
            BlendMode = StateBlendMode.Modulate;
        }

        public VertexShaderEffect(Func<VIn, VOut> function)
            : this(new VertexShaderState(function))
        {
        }

        public VertexShaderEffect(ShaderSource functionSource)
            : this(new VertexShaderState(functionSource))
        {
        }

        protected override VertexShaderState Blend(VertexShaderState previus)
        {
            switch (this.BlendMode)
            {
                case StateBlendMode.Modulate:
                    VertexShaderState blended = new VertexShaderState(state.lastFunction.Value, previus.lastFunction);
                    return blended;
                default: return this.state;
            }
        }

        public static implicit operator VertexShaderEffect<VIn, VOut>(Func<VIn, VOut> _delegate)
        {
            return new VertexShaderEffect<VIn, VOut>(new VertexShaderState(_delegate));
        }
    }

    public class PixelShaderEffect<PIn, POut> : BlendingEffect<PixelShaderState>
        where PIn : struct
        where POut : struct
    {
        PixelShaderEffect() : base() { }

        PixelShaderEffect(PixelShaderState state) : base(state) { }

        public PixelShaderEffect(Func<PIn, POut> function)
            : base(new PixelShaderState(function))
        {
            BlendMode = StateBlendMode.Modulate;
        }

        public PixelShaderEffect(ShaderSource functionSource)
            : this(new PixelShaderState(functionSource))
        {
        }

        public static implicit operator PixelShaderEffect<PIn, POut>(Func<PIn, POut> _delegate)
        {
            return new PixelShaderEffect<PIn, POut>(new PixelShaderState(_delegate));
        }

        protected override PixelShaderState Blend(PixelShaderState previus)
        {
            switch (this.BlendMode)
            {
                case StateBlendMode.Modulate:
                    PixelShaderState blended = new PixelShaderState(state.lastFunction.Value, previus.lastFunction);
                    return blended;
                default: return this.state;
            }
        }

    }
}
