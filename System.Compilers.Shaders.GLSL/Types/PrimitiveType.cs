using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.Types
{
  public abstract class PrimitiveType : GLSLType
  {
    protected PrimitiveType(string name)
      : base(name)
    {
    }

    public override bool Equals(GLSLType other)
    {
      if (Object.ReferenceEquals(other, null))
        return false;
      return GetTypeCode() == other.GetTypeCode();
    }
  }
}
