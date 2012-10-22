using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Utils;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public abstract class EqualityExpressionAST : BooleanExpressionAST
  {
    protected EqualityExpressionAST()
    {
    }

    protected EqualityExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    protected EqualityExpressionAST(ExpressionAST firtOp, ExpressionAST secondOp, int line, int column)
      : base(firtOp, secondOp, line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
    {
      context.MarkErrors();
      FirstOp.CheckSemantic(context);
      SecondOp.CheckSemantic(context);
      if (!context.CheckForErrors())
      {
        if (!FirstOp.Type.Equals(SecondOp.Type))
        {
          if (SecondOp.Type.ImplicitConvert(FirstOp.Type))
            context.Warnings.Add(new ImplicitConversionWarning(SecondOp.Type.Name, FirstOp.Type.Name, Line, Column));
          else if (FirstOp.Type.ImplicitConvert(SecondOp.Type))
            context.Warnings.Add(new ImplicitConversionWarning(FirstOp.Type.Name, SecondOp.Type.Name, Line, Column));
          else
            context.Errors.Add(new CannotAppliedOperatorError(GetOperatorToken(), FirstOp.Type.Name, SecondOp.Type.Name, Line, Column));
        }
      }
      Type = GLSLTypes.BoolType;
      context.UnMarkErrors();
    }
  }
}
