using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Compilers.Shaders.Info
{
    public interface ShaderType : ShaderMember
    {
        bool IsArray { get; }

        int Rank { get; }

        int[] Ranks { get; }

        bool IsGenericType { get; }

        bool IsGenericTypeDefinition { get; }

        bool IsGenericParameter { get; }

        ShaderType[] GetGenericParameters();

        ShaderType GetGenericDefinition();

        ShaderType BaseType { get; }

        ShaderType ElementType { get; }

        IEnumerable<ShaderMember> Members { get; }

        ShaderType MakeGenericType(params ShaderType[] typeArguments);
    }
}


