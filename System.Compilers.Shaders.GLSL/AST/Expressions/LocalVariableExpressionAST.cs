using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.AST.Declarations;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Expressions
{
  public class LocalVariableExpressionAST : LValueExpressionAST
  {
    public LocalVariableExpressionAST()
    {
    }

    public LocalVariableExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public LocalVariableExpressionAST(string name, int line, int column)
      : base(name, line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
    {
      VariableInfo varInfo;
      if (!context.Scope.TryGetVariableInfo(Name, out varInfo))
        context.Errors.Add(new VariableNotDeclaredError(Name, Line, Column));
      else
      {
        Type = varInfo.Type;
        VarInfo = varInfo;
      }
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      if (VarInfo.Is<LocalVariableInfo>())
        return VarInfo.Cast<LocalVariableInfo>().ConstantValue;
      if (VarInfo.Is<ParamInfo>())
        return VarInfo.Cast<ParamInfo>().DefaultValue;
      return null;
    }

    public override bool IsConstant
    {
      get { return VarInfo != null && VarInfo.IsConstant; }
    }

    public override bool IsWritable
    {
      get { return VarInfo != null && VarInfo.IsWritable; }
    }

    public VariableInfo VarInfo { get; set; }

  }
}
