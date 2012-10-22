using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.Info;

namespace System.Compilers.Shaders.ShaderAST
{
    public class ShaderFieldDeclarationAST : ShaderMemberDeclarationAST
    {
        public ShaderType FieldType { get; private set; }

        public Semantic Semantic { get; private set; }

        internal ShaderFieldDeclarationAST(ShaderProgramAST program, ShaderTypeDeclarationAST declaringType, ShaderType fieldType, string name, Semantic semantic)
            : base(program, declaringType, name)
        {
            this.FieldType = fieldType;
            this.Semantic = semantic;
            this.Field = new UserShaderField(this);
        }

        public ShaderField Field
        {
            get;
            private set;
        }

        public override ShaderMember Member
        {
            get { return Field; }
        }
    }
}
