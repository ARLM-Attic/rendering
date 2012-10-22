using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.Types
{
  public sealed class Sampler1DShadow : ObjectType
  {
    public const string Sampler1DShadowTypeName = "sampler1DShadow";

    public Sampler1DShadow()
      : base(Sampler1DShadowTypeName)
    {
    }

    public override GLSLTypeCode GetTypeCode()
    {
      return GLSLTypeCode.Sampler1DShadow;
    }

    public override bool ImplicitConvert(GLSLType type)
    {
      return false;
    }
  }
}
