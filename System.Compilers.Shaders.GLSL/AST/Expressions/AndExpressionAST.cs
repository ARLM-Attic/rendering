using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public class AndExpressionAST : LogicalExpressionAST
  {
    public AndExpressionAST()
    {
    }

    public AndExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public AndExpressionAST(ExpressionAST firstOp, ExpressionAST secondOp, int line, int column)
      : base(firstOp, secondOp, line, column)
    {
    }

    public override string GetOperatorToken()
    {
      return "&&";
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      return new BoolTypeInstance()
      {
        Value = (bool)FirstOp.GetConstantValueInternal().Value && (bool)SecondOp.GetConstantValueInternal().Value
      };
    }
  }
}
