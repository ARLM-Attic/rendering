using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Compilers.Shaders.AST;

namespace System.Compilers.Shaders.Reflection
{
    /// <summary>
    /// Represents a generic member of a shader language. I.e. Functions, types, variables, etc.
    /// </summary>
    public abstract class ShaderMember
    {
        internal ShaderMember()
        {
        }

        /// <summary>
        /// Gets when this member is a global function, variable or inner type.
        /// </summary>
        public abstract bool IsGlobal { get; }

        /// <summary>
        /// Gets the name for this member.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets if this member is primitive or not.
        /// </summary>
        public abstract bool IsPrimitive { get; }
    }

    public interface IPrimitiveMember <M> where M : MemberInfo
    {
        M NetMember { get; }
    }

    public interface IUserMember<M> where M : DeclarationAST
    {
        M DeclarationAST { get; }
    }
}
