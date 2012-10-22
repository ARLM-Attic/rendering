using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.Types
{
  public abstract class ScalarType : PrimitiveType
  {
    public ScalarType(string name)
      : base(name)
    {
    }

    public override int GetNumberOfComponents()
    {
      return 1;
    }

    public override GLSLType GetTypeBase()
    {
      return this;
    }
  }
}
