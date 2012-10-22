using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Utils;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public class MinusExpressionAST : UnaryExpressionAST
  {
    protected MinusExpressionAST()
    {
    }

    protected MinusExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public MinusExpressionAST(ExpressionAST postfix, int line, int column)
      : base(postfix, line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
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

    public override string GetOperatorToken()
    {
      return "-";
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      return -Operand.GetConstantValue();
    }

    public override bool IsConstant
    {
      get { return Operand.IsConstant; }
    }
  }
}
