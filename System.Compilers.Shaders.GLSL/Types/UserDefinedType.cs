using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Declarations;

namespace GLSLCompiler.Types
{
  public abstract class UserDefinedType : GLSLType
  {
    public UserDefinedType(string name)
      : base(name)
    {
    }

    public override bool Equals(GLSLType other)
    {
      return Object.ReferenceEquals(this, other);
    }
  }
}
