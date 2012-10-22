using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Compilers.Shaders;

namespace System.Rendering.Effects.Shaders
{
    class LambdaExpressionShaderBuilderAgent : ShaderBuilderAgent
    {
        public override bool CanBuild(ShaderSource shaderSource)
        {
            return shaderSource is ShaderSource.ShaderSourceLambdaExpression;
        }

        public override void Build(ShaderSource shaderSource, Builtins builtins)
        {
            if (!(shaderSource is ShaderSource.ShaderSourceLambdaExpression))
                throw new ArgumentOutOfRangeException("Expression lambda source expected.");

            /// TODO: Implement here a real agent.
            DelegateShaderBuilderAgent agent = new DelegateShaderBuilderAgent();
            agent.Build(new ShaderSource.ShaderSourceDelegate { Delegate = (((LambdaExpression)((ShaderSource.ShaderSourceLambdaExpression)shaderSource).Function).Compile()) }, builtins);

            this.Program = agent.Program;
        }
    }
}
