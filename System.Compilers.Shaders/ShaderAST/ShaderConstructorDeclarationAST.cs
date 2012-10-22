using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.Info;

namespace System.Compilers.Shaders.ShaderAST
{
    public class ShaderConstructorDeclarationAST : ShaderMethodBaseDeclarationAST
    {
        internal ShaderConstructorDeclarationAST(ShaderTypeDeclarationAST declaringType) :
            base(declaringType.Program, declaringType, "ctor")
        {
            this.Constructor = new UserShaderConstructor(this);
        }

        public ShaderConstructor Constructor
        {
            get;
            private set;
        }

        public override ShaderMember Member
        {
            get { return Constructor; }
        }
    }
}
