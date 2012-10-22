using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Expressions
{
  public abstract class RelationalExpressionAST : BooleanExpressionAST
  {
    protected RelationalExpressionAST()
    {
    }

    protected RelationalExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    protected RelationalExpressionAST(ExpressionAST firstOp, ExpressionAST secondOp, int line, int column)
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
        if (FirstOp.Type.IsFloat() && SecondOp.Type.IsInteger())
          context.Warnings.Add(new ImplicitConversionWarning(SecondOp.Type.Name, FirstOp.Type.Name, SecondOp.Line, SecondOp.Column));
        else if (FirstOp.Type.IsInteger() && SecondOp.Type.IsFloat())
          context.Warnings.Add(new ImplicitConversionWarning(FirstOp.Type.Name, SecondOp.Type.Name, FirstOp.Line, FirstOp.Column));
        else if (!FirstOp.Type.IsArithmeticType() || !SecondOp.Type.IsArithmeticType())
          context.Errors.Add(new CannotAppliedOperatorError(GetOperatorToken(), FirstOp.Type.Name, SecondOp.Type.Name, FirstOp.Line, FirstOp.Column));
      }
      Type = GLSLTypes.BoolType;
      context.UnMarkErrors();
    }
  }
}
