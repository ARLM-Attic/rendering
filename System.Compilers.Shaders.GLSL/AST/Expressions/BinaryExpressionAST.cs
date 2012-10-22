using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public abstract class BinaryExpressionAST : ExpressionAST, IOperator
  {
    public BinaryExpressionAST()
    {
    }

    public BinaryExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public BinaryExpressionAST(ExpressionAST firstOp, ExpressionAST secondOp, int line, int column)
      : base(line, column)
    {
      FirstOp = firstOp;
      SecondOp = secondOp;
    }

    public abstract string GetOperatorToken();

    public override bool IsConstant
    {
      get { return FirstOp.IsConstant && SecondOp.IsConstant; }
    }

    public ExpressionAST FirstOp { get; set; }

    public ExpressionAST SecondOp { get; set; }

  }
}
