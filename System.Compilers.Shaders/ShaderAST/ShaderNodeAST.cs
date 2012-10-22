using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.Info;

namespace System.Compilers.Shaders.ShaderAST
{
    public abstract class ShaderNodeAST
    {
        internal ShaderNodeAST(ShaderProgramAST program)
        {
            if (this is ShaderProgramAST)
                this.Program = this as ShaderProgramAST;
            else
                this.Program = program;

            if (this.Program == null)
                throw new ArgumentNullException("Argument null is only valid when creating program instance");
        }

        internal protected ShaderProgramAST Program { get; private set; }
    }
}
