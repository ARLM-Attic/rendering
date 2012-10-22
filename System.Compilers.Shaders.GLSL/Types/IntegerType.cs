using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Declarations;

namespace GLSLCompiler.Types
{
  public sealed class IntegerType : ScalarType
  {
    public const string IntegerTypeName = "int";

    public IntegerType()
      : base(IntegerTypeName)
    {
    }

    public override GLSLTypeCode GetTypeCode()
    {
      return GLSLTypeCode.Integer;
    }

    public override bool ImplicitConvert(GLSLType type)
    {
      GLSLTypeCode typeCode = type.GetTypeCode();
      switch (typeCode)
      {
        case GLSLTypeCode.Integer:
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
