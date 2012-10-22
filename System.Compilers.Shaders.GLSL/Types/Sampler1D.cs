using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.Types
{
  public sealed class Sampler1D : ObjectType
  {
    public const string Sampler1DTypeName = "sampler1D";

    public Sampler1D()
      : base(Sampler1DTypeName)
    {
    }

    public override GLSLTypeCode GetTypeCode()
    {
      return GLSLTypeCode.Sampler1D;
    }

    public override bool ImplicitConvert(GLSLType type)
    {
      return false;
    }
  }
}
