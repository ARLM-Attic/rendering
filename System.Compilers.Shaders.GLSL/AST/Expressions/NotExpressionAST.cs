using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Expressions
{
  public class NotExpressionAST : UnaryExpressionAST
  {
    protected NotExpressionAST()
    {
    }

    protected NotExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public NotExpressionAST(ExpressionAST postfix, int line, int column)
      : base(postfix, line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
    {
      context.MarkErrors();
      Operand.CheckSemantic(context);
      if (!context.CheckForErrors())
      {
        if (Operand.Type.IsBool())
          Type = Operand.Type;
        else
          context.Errors.Add(new CannotAppliedOperatorError(GetOperatorToken(), Operand.Type.Name, Operand.Line, Operand.Column));
      }
      context.UnMarkErrors();
    }

    public override string GetOperatorToken()
    {
      return "!";
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      return Operand != null ? !Operand.GetConstantValue() : null;
    }

    public override bool IsConstant
    {
      get { return Operand != null && Operand.IsConstant; }
    }
  }
}
