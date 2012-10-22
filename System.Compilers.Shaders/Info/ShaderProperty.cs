using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Compilers.Shaders.Info
{
    public interface ShaderProperty : ShaderMember
    {
        ShaderType PropertyType { get; }

        ShaderMethod GetMethod { get; }

        ShaderMethod SetMethod { get; }
    }
}
