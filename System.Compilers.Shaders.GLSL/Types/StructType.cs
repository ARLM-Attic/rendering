using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Declarations;
using GLSLCompiler.Utils;

namespace GLSLCompiler.Types
{
  public class StructType : UserDefinedType
  {
    public StructType(string name, IEnumerable<FieldInfo> fieldsInfo)
      : base(name)
    {
      FieldsInfo = new List<FieldInfo>(fieldsInfo);
    }

    public override GLSLTypeCode GetTypeCode()
    {
      return GLSLTypeCode.Struct;
    }

    public override bool ImplicitConvert(GLSLType type)
    {
      return Equals(type);
    }

    public override int GetNumberOfComponents()
    {
      return FieldsInfo.Count;
    }

    public override GLSLType GetTypeBase()
    {
      return this;
    }

    public override IEnumerable<GLSLType> GetFlatTypes()
    {
      foreach (var fInfo in FieldsInfo)
        yield return fInfo.Type;
    }

    public List<FieldInfo> FieldsInfo { get; private set; }

    public class FieldInfo
    {
      public FieldInfo(string name, GLSLType type)
      {
        Name = name;
        Type = type;
      }

      public string Name { get; private set; }

      public GLSLType Type { get; internal set; }
    }
  }
}
