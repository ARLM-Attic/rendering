using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Compilers.Shaders.ShaderAST;

namespace System.Compilers.Shaders.Info
{
    public interface ShaderMember
    {
        string Name { get; }

        bool IsBuiltin { get; }

        bool IsGlobal { get; }

        ShaderType DeclaringType { get; }
    }

    


}
