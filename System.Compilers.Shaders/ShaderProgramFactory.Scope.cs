using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.ShaderAST;
using System.Reflection;
using System.Compilers.Shaders.Info;

namespace System.Compilers.Shaders
{
    partial class ShaderProgramFactory
    {
        class Scope
        {
            public Builtins Builtins { get; private set; }

            Dictionary<MemberInfo, ShaderMember> compiled = new Dictionary<MemberInfo, ShaderMember>();

            public Scope(Builtins builtins)
            {
                this.Builtins = builtins;
            }

            public bool Contains(MemberInfo member)
            {
                var m = Builtins.Resolve(member);
                if (m != null)
                    return true;

                return compiled.ContainsKey(member);
            }

            public ShaderMember this[MemberInfo member]
            {
                get
                {
                    var m = Builtins.Resolve(member);
                    if (m != null)
                        return m;
                    return compiled[member];
                }
            }

            public void Add(MemberInfo member, ShaderMember shaderMember)
            {
                compiled[member] = shaderMember;
            }

            Stack<ShaderTypeDeclarationAST> declaringTypes = new Stack<ShaderTypeDeclarationAST>();

            public ShaderTypeDeclarationAST CurrentDeclaringType { get { return declaringTypes.Peek(); } }

            public ShaderMethodBaseDeclarationAST CurrentDeclaringMethod { get; set; }

            public void BeginType(ShaderTypeDeclarationAST declaringType)
            {
                declaringTypes.Push(declaringType);
            }

            public void EndType()
            {
                declaringTypes.Pop();
            }

            public MethodInfo MainMethod
            {
                get;
                set;
            }
        }


    }
}
