using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.Types
{
  public class ObjectType : PrimitiveType
  {
    public ObjectType(string name)
      : base(name)
    {
    }

    public override bool ImplicitConvert(GLSLType type)
    {
      return Equals(type);
    }

    public override GLSLTypeCode GetTypeCode()
    {
      throw new NotImplementedException();
    }

    public override int GetNumberOfComponents()
    {
      return 1;
    }

    public override GLSLType GetTypeBase()
    {
      return this;
    }

    public override IEnumerable<GLSLType> GetFlatTypes()
    {
      yield break;
    }
  }
}
