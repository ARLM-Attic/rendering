using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Expressions
{
  public abstract class LogicalExpressionAST : BooleanExpressionAST
  {
    protected LogicalExpressionAST()
    {
    }

    protected LogicalExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    protected LogicalExpressionAST(ExpressionAST firstOp, ExpressionAST secondOp, int line, int column)
      : base(firstOp, secondOp, line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
    {
      context.MarkErrors();
      FirstOp.CheckSemantic(context);
      SecondOp.CheckSemantic(context);
      if (!context.CheckForErrors())
      {
        if (!FirstOp.Type.IsBool() || !SecondOp.Type.IsBool())
          context.Errors.Add(new CannotAppliedOperatorError(GetOperatorToken(), FirstOp.Type.Name, SecondOp.Type.Name, FirstOp.Line, FirstOp.Column));
      }
      Type = GLSLTypes.BoolType;
      context.UnMarkErrors();
    }
  }
}
