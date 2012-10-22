//#define DEPLOYING_IL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Compilers.Shaders;

namespace System.Rendering.Effects.Shaders
{
#if DEPLOYING_IL
    public 
#endif
        class DelegateShaderBuilderAgent : ShaderBuilderAgent
    {
        public DelegateShaderBuilderAgent()
        {
        }

        public override bool CanBuild(ShaderSource shaderSource)
        {
            return shaderSource is ShaderSource.ShaderSourceDelegate;
        }

        public override void Build(ShaderSource shaderSource, Builtins builtins)
        {
            if (shaderSource == null)
                throw new ArgumentNullException();

            if (shaderSource.GetType() != typeof(ShaderSource.ShaderSourceDelegate))
                throw new ArgumentOutOfRangeException("type must be delegate");

            ShaderSource.ShaderSourceDelegate del = shaderSource as ShaderSource.ShaderSourceDelegate;

            Program = ShaderProgramFactory.Build(del.Delegate.Method, builtins);

            Target = del.Target;
        }
    }
}
