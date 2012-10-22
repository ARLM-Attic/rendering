using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.Types
{
  public sealed class VoidType : PrimitiveType
  {
    public VoidType()
      : base("void")
    {
    }

    public override GLSLTypeCode GetTypeCode()
    {
      return GLSLTypeCode.Void;
    }

    public override bool ImplicitConvert(GLSLType type)
    {
      return type.GetTypeCode() == GLSLTypeCode.Void;
    }

    public override int GetNumberOfComponents()
    {
      return -1;
    }

    public override GLSLType GetTypeBase()
    {
      return this;
    }

    public override IEnumerable<GLSLType> GetFlatTypes()
    {
      yield return this;
    }
  }
}
