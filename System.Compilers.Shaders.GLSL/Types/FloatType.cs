using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Declarations;

namespace GLSLCompiler.Types
{
  public sealed class FloatType : ScalarType
  {
    public const string FloatTypeName = "float";

    public FloatType()
      : base(FloatTypeName)
    {
    }

    public override GLSLTypeCode GetTypeCode()
    {
      return GLSLTypeCode.Float;
    }

    public override bool ImplicitConvert(GLSLType type)
    {
      GLSLTypeCode typeCode = type.GetTypeCode();
      switch (typeCode)
      {
        case GLSLTypeCode.Float:
          return true;
        default:
          return false;
      }
    }

    public override IEnumerable<GLSLType> GetFlatTypes()
    {
      yield return this;
    }
  }
}
