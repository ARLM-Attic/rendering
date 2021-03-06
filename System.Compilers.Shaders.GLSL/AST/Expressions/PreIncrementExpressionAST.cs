﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Utils;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public class PreIncrementExpressionAST : UnaryExpressionAST
  {
    protected PreIncrementExpressionAST()
    {
    }

    protected PreIncrementExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public PreIncrementExpressionAST(ExpressionAST postfix, int line, int column)
      : base(postfix, line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
    {
      if (!Operand.IsLValue())
        context.Errors.Add(new CompilerError("The operand of an increment or decrement must be an lvalue", Operand.Line, Operand.Column));
      else
      {
        context.MarkErrors();
        Operand.CheckSemantic(context);
        if (!context.CheckForErrors())
        {
          if (Operand.Type.IsArithmeticTypeBased())
            Type = Operand.Type;
          else
            context.Errors.Add(new CannotAppliedOperatorError(GetOperatorToken(), Operand.Type.Name, Operand.Line, Operand.Column));
        }
        context.UnMarkErrors();
      }
    }

    public override string GetOperatorToken()
    {
      return "++";
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      throw new NotImplementedException();
    }

    public override bool IsConstant
    {
      get { return false; }
    }
  }
}
