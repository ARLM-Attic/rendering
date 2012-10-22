using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Utils;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public class ConditionExpressionAST : NaryExpressionAST, IOperator
  {
    public ConditionExpressionAST()
    {
    }

    public ConditionExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public ConditionExpressionAST(ExpressionAST cond, ExpressionAST ontrue, ExpressionAST onfalse, int line, int column)
      : base(new [] { cond, ontrue, onfalse }, line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
    {
      context.MarkErrors();
      Condition.CheckSemantic(context);
      if (!context.CheckForErrors())
      {
        if (!Condition.Type.IsBool())
          context.Errors.Add(new CannotImplicitConvertError(Condition.Type.Name, GLSLTypes.BoolType.Name, Condition.Line, Condition.Column));
      }
      context.UnMarkErrors();

      context.MarkErrors();
      OnTrue.CheckSemantic(context);
      OnFalse.CheckSemantic(context);
      if (!context.CheckForErrors())
      {
        if (!OnTrue.Type.Equals(OnFalse.Type))
        {
          if (OnTrue.Type.ImplicitConvert(OnFalse.Type))
          {
            context.Warnings.Add(new ImplicitConversionWarning(OnTrue.Type.Name, OnFalse.Type.Name, OnTrue.Line, OnTrue.Column));
            Type = OnTrue.Type;
          }
          else if (OnFalse.Type.ImplicitConvert(OnTrue.Type))
          {
            context.Warnings.Add(new ImplicitConversionWarning(OnFalse.Type.Name, OnTrue.Type.Name, OnFalse.Line, OnFalse.Column));
            Type = OnFalse.Type;
          }
          else
            context.Errors.Add(new CannotImplicitConvertError(OnTrue.Type.Name, OnFalse.Type.Name, Line, Column));
        }
        else
          Type = OnTrue.Type;
      }
      context.UnMarkErrors();
    }

    public string GetOperatorToken()
    {
      return "?:";
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      return Condition.GetConstantValue<bool>() ? OnTrue.GetConstantValueInternal() : OnFalse.GetConstantValueInternal();
    }

    public override bool IsConstant
    {
      get { return Condition.IsConstant && OnTrue.IsConstant && OnFalse.IsConstant; }
    }

    public ExpressionAST Condition
    {
      get { return Operands[0]; }
      set { Operands[0] = value; }
    }

    public ExpressionAST OnTrue
    {
      get { return Operands[1]; }
      set { Operands[1] = value; }
    }

    public ExpressionAST OnFalse 
    {
      get { return Operands[2]; }
      set { Operands[2] = value; }
    }
  }
}
