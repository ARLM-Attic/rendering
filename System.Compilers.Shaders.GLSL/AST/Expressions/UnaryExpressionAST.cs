using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public abstract class UnaryExpressionAST : ExpressionAST, IOperator
  {
    protected UnaryExpressionAST()
    {
    }

    protected UnaryExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    protected UnaryExpressionAST(ExpressionAST op, int line, int column)
      : base(line, column)
    {
      Operand = op;
    }

    public abstract string GetOperatorToken();

    public ExpressionAST Operand { get; set; }
  }
}
