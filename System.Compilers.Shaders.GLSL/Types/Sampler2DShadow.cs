using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.Types
{
  public sealed  class Sampler2DShadow : ObjectType
  {
    public const string Sampler2DShadowTypeName = "sampler2DShadow";

    public Sampler2DShadow()
      : base(Sampler2DShadowTypeName)
    {
    }

    public override GLSLTypeCode GetTypeCode()
    {
      return GLSLTypeCode.Sampler2DShadow;
    }

    public override bool ImplicitConvert(GLSLType type)
    {
      return false;
    }
  }
}
