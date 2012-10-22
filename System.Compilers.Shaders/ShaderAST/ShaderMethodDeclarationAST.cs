using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.Info;

namespace System.Compilers.Shaders.ShaderAST
{
    public class ShaderMethodDeclarationAST : ShaderMethodBaseDeclarationAST
    {
        /// <summary>
        /// Gets or sets the return type of this method.
        /// </summary>
        public ShaderType ReturnType { get; internal set; }

        internal ShaderMethodDeclarationAST(ShaderProgramAST program, ShaderTypeDeclarationAST declaringType, string name, bool isAbstract,
            int numberOfGenericParameters)
            : base(program, declaringType, name, isAbstract)
        {
            this.NumberOfGenericParameters = numberOfGenericParameters;
            this.Method = new UserShaderMethod(this);
        }

        public ShaderMethod Method
        {
            get;
            private set;
        }

        public override ShaderMember Member
        {
            get { return Method; }
        }

        public int NumberOfGenericParameters { get; private set; }
    }
}
