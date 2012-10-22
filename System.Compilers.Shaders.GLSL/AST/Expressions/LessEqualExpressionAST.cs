using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public class LessEqualExpressionAST : RelationalExpressionAST
  {
    public LessEqualExpressionAST()
    {
    }

    public LessEqualExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public LessEqualExpressionAST(ExpressionAST firstop, ExpressionAST secondop, int line, int column)
      : base(firstop, secondop, line, column)
    {
    }

    public override string GetOperatorToken()
    {
      return "<=";
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      return FirstOp.GetConstantValueInternal() <= SecondOp.GetConstantValueInternal();
    }
  }
}
