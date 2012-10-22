using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.Types
{
  public sealed class SamplerCube : ObjectType
  {
    public const string SamplerCubeTypeName = "samplerCube";

    public SamplerCube()
      : base(SamplerCubeTypeName)
    {
    }

    public override GLSLTypeCode GetTypeCode()
    {
      return GLSLTypeCode.SamplerCube;
    }

    public override bool ImplicitConvert(GLSLType type)
    {
      return false;
    }
  }
}
