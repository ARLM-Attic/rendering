using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.Types
{
  public sealed class Sampler2D : ObjectType
  {
    public const string Sampler2DTypeName = "sampler2D";

    public Sampler2D() : base(Sampler2DTypeName)
    {

    }

    public override GLSLTypeCode GetTypeCode()
    {
      return GLSLTypeCode.Sampler2D;
    }

    public override bool ImplicitConvert(GLSLType type)
    {
      return false;
    }
  }
}
