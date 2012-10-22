using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.Info;

namespace System.Compilers.Shaders.ShaderAST
{
    public abstract class ShaderMemberDeclarationAST : ShaderNodeAST
    {
        public ShaderTypeDeclarationAST DeclaringType
        {
            get;
            private set;
        }

        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets when this member is meant to be global (shared, static).
        /// </summary>
        public bool IsGlobal { get; set; }

        public abstract ShaderMember Member { get; }

        internal ShaderMemberDeclarationAST(ShaderProgramAST program, ShaderTypeDeclarationAST declaringType, string name)
            :base (program)
        {
            if (declaringType != null)
                if (declaringType.Program != program)
                    throw new ArgumentException("Declaring type should be of same program than member");

            this.DeclaringType = declaringType;
            this.Name = name;
        }
    }
}
