using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.Info;

namespace System.Compilers.Shaders.ShaderAST
{
    public abstract class ShaderMethodBaseDeclarationAST : ShaderMemberDeclarationAST
    {
        private List<ShaderParameter> parameters = new List<ShaderParameter>();

        public IEnumerable<ShaderParameter> Parameters { get { return parameters; } }

        private ShaderMethodBodyAST _Body;

        internal ShaderMethodBaseDeclarationAST(ShaderProgramAST program, ShaderTypeDeclarationAST declaringType, string name, bool isAbstract)
            : base(program, declaringType, name)
        {
            if (!isAbstract)
                Implement();
        }

        internal void Implement()
        {
            _Body = new ShaderMethodBodyAST(this);
        }

        public bool IsAbstract { get { return _Body == null; } }

        internal ShaderMethodBaseDeclarationAST (ShaderProgramAST program, ShaderTypeDeclarationAST declaringType, string name)
            : this (program, declaringType, name, false)            
        {
        }

        public ShaderMethodBodyAST Body { get { return _Body; } internal set { _Body = value; } }

        public ShaderParameter DeclareNewParameter(ShaderType parameterType, string name, Semantic semantic, ParameterModifier modifier)
        {
            var parameter = new ShaderParameter(parameterType, name, semantic, modifier);

            parameters.Add(parameter);

            return parameter;
        }

        public ShaderMethodBuilder GetMethodBuilder()
        {
            if (this.IsAbstract)
                Implement();

            return new ShaderMethodBuilder(this);
        }

        public int NumberOfParameters { get { return parameters.Count; } }
    }
}
