using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public class DivExpressionAST : ArithmeticExpressionAST
  {
    public DivExpressionAST()
    {
    }

    public DivExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public DivExpressionAST(ExpressionAST firstOp, ExpressionAST secondOp, int line, int column)
      : base(firstOp, secondOp, line, column)
    {
    }

    public override string GetOperatorToken()
    {
      return "/";
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      return FirstOp.GetConstantValueInternal() / SecondOp.GetConstantValueInternal();
    }
  }
}
