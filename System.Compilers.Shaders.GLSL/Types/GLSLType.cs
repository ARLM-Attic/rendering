using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Declarations;

namespace GLSLCompiler.Types
{
  public abstract class GLSLType : IEquatable<GLSLType>
  {
    protected GLSLType(string name)
    {
      Name = name;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      return Equals(obj as GLSLType);
    }

    public abstract bool Equals(GLSLType other);

    public override int GetHashCode()
    {
      return Name.GetHashCode();
    }

    public abstract GLSLTypeCode GetTypeCode();

    public abstract GLSLType GetTypeBase();

    public abstract int GetNumberOfComponents();

    public abstract IEnumerable<GLSLType> GetFlatTypes();

    public abstract bool ImplicitConvert(GLSLType type);

    public override string ToString()
    {
      return Name;
    }

    public string Name { get; protected set; }

  }
}
