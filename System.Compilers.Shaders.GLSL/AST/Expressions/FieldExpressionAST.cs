using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using System.Reflection.Emit;
using GLSLCompiler.AST.Declarations;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Expressions
{
  public class FieldExpressionAST : LValueExpressionAST
  {
    public FieldExpressionAST()
    {
    }

    public FieldExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public FieldExpressionAST(string name, ExpressionAST lvalue, int line, int column)
      : base(name, line, column)
    {
      LValue = lvalue;
    }

    public override void CheckSemantic(SemanticContext context)
    {
      context.MarkErrors();
      LValue.CheckSemantic(context);
      if (!context.CheckForErrors())
      {
        if (LValue.Type.IsStruct())
        {
          StructType sType = LValue.Type.Cast<StructType>();
          StructType.FieldInfo fInfo = sType.FieldsInfo.FirstOrDefault(f => f.Name == Name);
          if (fInfo == null)
            context.Errors.Add(new TypeDoesNotContainsFieldError(sType.Name, Name, Line, Column));
          else
            Type = fInfo.Type;
        }
        else if (LValue.Type.IsVec())
        {
          VecType vType = LValue.Type.Cast<VecType>();
          char[] components = Name.ToCharArray();
          VecType.VecFieldGroups group = (VecType.VecFieldGroups)0;
          for (int i = 0; i < components.Length; i++)
          {
            context.MarkErrors();
            if(!vType.IsValidComponent(components[i]))
              context.Errors.Add(new TypeDoesNotContainsFieldError(vType.Name, Name, Line, Column));
            else if (!vType.IsLargeEnough(components[i]))
              context.Errors.Add(new SemanticError("Cannot access to component {0} for type {1}".Fmt(components[i], vType), Line, Column));
            else if (group != 0 && !vType.GroupContains(components[i], group))
              context.Errors.Add(new SemanticError("Cannot mix distinct group of components accesors", Line, Column));
            else
              group = vType.GetGroupFromComponent(components[i]);
            if (context.CheckForErrors())
            {
              context.UnMarkErrors();
              break;
            }
          }
          if (components.Length == 1)
            Type = vType.ElementType;
          else
          {
            GLSLTypeCode tCode = Enum.Parse(typeof(GLSLTypeCode), vType.ElementType.Name, true).Cast<GLSLTypeCode>();
            Type = Helper.GetType("{0}Vec{1}", tCode.ToString(), components.Length);
          }
        }
        else
          context.Errors.Add(new SemanticError("Type {0} doesn't contains a field named {1}".Fmt(LValue.Type, Name), Line, Column));
      }
      context.UnMarkErrors();
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      return LValue.GetConstantValue().DotOverride(Name);
    }

    public ExpressionAST LValue { get; set; }

    public override bool IsConstant
    {
      get { return LValue.IsConstant; }
    }

    public override bool IsWritable
    {
      get 
      {
        if (LValue.Type.IsVec())
        {
          VecType vType = LValue.Type.Cast<VecType>();
          char[] components = Name.ToCharArray();
          VecType.VecFieldGroups group = (VecType.VecFieldGroups)0;
          for (int i = 0; i < components.Length; i++)
          {
            if (!vType.IsValidComponent(components[i]))
              return false;
            else if (!vType.IsLargeEnough(components[i]))
              return false;
            else if (group != 0 && vType.GroupContains(components[i], group))
              return false;
            else
              group = vType.GetGroupFromComponent(components[i]);
          }
          return !LValue.IsConstant;
        }
        else if (LValue.Type.IsStruct())
          return !LValue.IsConstant;
        else
          return false;
      }
    }

  }
}
