using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Effects.Shaders;
using System.Rendering.RenderStates;

namespace System.Rendering.Effects
{
    public abstract class ShaderEffect<ShaderState> : AppendableEffect<ShaderState> where ShaderState : struct
    {
        public ShaderEffect(ShaderState state)
            : base(state)
        {
        }
    }

    public class VertexShaderEffect <VIn, VOut> : ShaderEffect<VertexShaderState>
        where VIn : struct
        where VOut : struct
    {
        VertexShaderEffect(VertexShaderState state)
            : base(state)
        {
            AppendMode = Effects.AppendMode.Prepend;
        }

        public VertexShaderEffect(Func<VIn, VOut> function)
            : this(new VertexShaderState(function))
        {
        }

        public VertexShaderEffect(ShaderSource functionSource)
            : this(new VertexShaderState(functionSource))
        {
        }

        protected override VertexShaderState Append(VertexShaderState previous)
        {
            switch (this.AppendMode)
            {
                case AppendMode.Append:
                    return VertexShaderState.Join(previous, this.state);
                case Effects.AppendMode.Prepend:
                    return VertexShaderState.Join(this.state, previous);
                case Effects.AppendMode.None:
                    return previous;
                case Effects.AppendMode.Replace:
                    return this.state;
                default: return this.state;
            }
        }

        public static implicit operator VertexShaderEffect<VIn, VOut>(Func<VIn, VOut> _delegate)
        {
            return new VertexShaderEffect<VIn, VOut>(new VertexShaderState(_delegate));
        }
    }

    public class PixelShaderEffect<PIn, POut> : ShaderEffect<PixelShaderState>
        where PIn : struct
        where POut : struct
    {
        PixelShaderEffect(PixelShaderState state) : base(state) { }

        public PixelShaderEffect(Func<PIn, POut> function)
            : base(new PixelShaderState(function))
        {
            AppendMode = AppendMode.Append;
        }

        public PixelShaderEffect(ShaderSource functionSource)
            : this(new PixelShaderState(functionSource))
        {
        }

        public static implicit operator PixelShaderEffect<PIn, POut>(Func<PIn, POut> _delegate)
        {
            return new PixelShaderEffect<PIn, POut>(new PixelShaderState(_delegate));
        }

        protected override PixelShaderState Append(PixelShaderState previous)
        {
            switch (this.AppendMode)
            {
                case AppendMode.Append:
                    return PixelShaderState.Join(previous, this.state);
                case Effects.AppendMode.Prepend:
                    return PixelShaderState.Join(this.state, previous);
                case Effects.AppendMode.None:
                    return previous;
                case Effects.AppendMode.Replace:
                    return state;
                default: return this.state;
            }
        }
    }
}
