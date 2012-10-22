using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public class GreaterExpressionAST : RelationalExpressionAST
  {
    public GreaterExpressionAST()
    {
    }

    public GreaterExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public GreaterExpressionAST(ExpressionAST firstop, ExpressionAST secondop, int line, int column)
      : base(firstop, secondop, line, column)
    {
    }

    public override string GetOperatorToken()
    {
      return ">";
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      return FirstOp.GetConstantValueInternal() > SecondOp.GetConstantValueInternal();
    }
  }
}
