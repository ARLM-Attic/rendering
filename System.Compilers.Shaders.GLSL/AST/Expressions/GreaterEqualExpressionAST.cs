using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public class GreaterEqualExpressionAST : RelationalExpressionAST
  {
    public GreaterEqualExpressionAST()
    {
    }

    public GreaterEqualExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public GreaterEqualExpressionAST(ExpressionAST firstop, ExpressionAST secondop, int line, int column)
      : base(firstop, secondop, line, column)
    {
    }

    public override string GetOperatorToken()
    {
      return ">=";
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      return FirstOp.GetConstantValueInternal() >= SecondOp.GetConstantValueInternal();
    }
  }
}
