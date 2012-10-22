using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.AST.Expressions
{
  public class MulExpressionAST : ArithmeticExpressionAST
  {
    public MulExpressionAST()
    {
    }

    public MulExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public MulExpressionAST(ExpressionAST firstOp, ExpressionAST secondOp, int line, int column)
      : base(firstOp, secondOp, line, column)
    {
    }

    public override string GetOperatorToken()
    {
      return "*";
    }

    internal override global::GLSLCompiler.Types.TypeInstance GetConstantValueInternal()
    {
      return FirstOp.GetConstantValueInternal() * SecondOp.GetConstantValueInternal();
    }
  }
}
