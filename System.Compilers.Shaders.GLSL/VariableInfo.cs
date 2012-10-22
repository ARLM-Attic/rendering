using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler
{
  public abstract class VariableInfo
  {
    public string Name { get; set; }

    public GLSLType Type { get; set; }

    public abstract bool IsWritable { get; }

    public abstract bool IsConstant { get; }
  }

  public class LocalVariableInfo : VariableInfo
  {
    public TypeQualifier Qualifier { get; set; }

    public override bool IsWritable
    {
      get
      {
        switch (Qualifier)
        {
          case TypeQualifier.Const:
          case TypeQualifier.Attribute:
          case TypeQualifier.Uniform:
            return false;
          default:
            return true;
        }
      }
    }

    public override bool IsConstant
    {
      get { return Qualifier == TypeQualifier.Const; }
    }

    public TypeInstance ConstantValue { get; set; }
  }

  public class FieldVariableInfo : VariableInfo
  {
    public override bool IsWritable
    {
      get { return true; }
    }

    public override bool IsConstant
    {
      get { return false; }
    }
  }

  public class ParamInfo : VariableInfo
  {
    public ParamQualifier Qualifier { get; set; }

    public bool HasDefaultValue 
    {
      get { return Object.ReferenceEquals(DefaultValue , null); }
    }

    public TypeInstance DefaultValue { get; set; }

    public override bool IsWritable
    {
      get
      {
        switch (Qualifier)
        {
          case ParamQualifier.Const:
          case ParamQualifier.Const_In:
            return false;
          default:
            return true;
        }
      }
    }

    public override bool IsConstant
    {
      get
      {
        switch (Qualifier)
        {
          case ParamQualifier.Const:
          case ParamQualifier.Const_In:
            return true;
          default:
            return false;
        }
      }
    }
  }
}
