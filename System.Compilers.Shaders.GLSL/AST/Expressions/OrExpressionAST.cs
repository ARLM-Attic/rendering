using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public class OrExpressionAST : LogicalExpressionAST
  {
    public OrExpressionAST()
    {
    }

    public OrExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public OrExpressionAST(ExpressionAST firstOp, ExpressionAST secondOp, int line, int column)
      : base(firstOp, secondOp, line, column)
    {
    }

    public override string GetOperatorToken()
    {
      return "||";
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      return new BoolTypeInstance()
      {
        Value = (bool)FirstOp.GetConstantValueInternal().Value || (bool)SecondOp.GetConstantValueInternal().Value
      };
    }
  }
}
