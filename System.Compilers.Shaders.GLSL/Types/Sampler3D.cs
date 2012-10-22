using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.Types
{
  public sealed class Sampler3D : ObjectType
  {
    public const string Sampler3DTypeName = "sampler3D";

    public Sampler3D()
      : base(Sampler3DTypeName)
    {

    }

    public override GLSLTypeCode GetTypeCode()
    {
      return GLSLTypeCode.Sampler3D;
    }

    public override bool ImplicitConvert(GLSLType type)
    {
      return false;
    }
  }
}
