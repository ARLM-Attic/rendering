using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public abstract class BooleanExpressionAST : BinaryExpressionAST
  {
    protected BooleanExpressionAST()
    {
    }

    protected BooleanExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    protected BooleanExpressionAST(ExpressionAST firstOp, ExpressionAST secondOp, int line, int column)
      : base(firstOp, secondOp, line, column)
    {
    }
  }
}
