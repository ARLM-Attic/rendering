using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.Types
{
  public sealed class BoolType : ScalarType
  {
    public const string BoolTypeName = "bool";

    public BoolType()
      : base(BoolTypeName)
    {
      
    }

    public override GLSLTypeCode GetTypeCode()
    {
      return GLSLTypeCode.Bool;
    }

    public override bool ImplicitConvert(GLSLType type)
    {
      GLSLTypeCode tCode = type.GetTypeCode();
      switch (tCode)
      {
        case GLSLTypeCode.Bool:
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
