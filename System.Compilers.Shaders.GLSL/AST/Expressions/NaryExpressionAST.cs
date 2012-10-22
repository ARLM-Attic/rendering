using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Expressions
{
  public abstract class NaryExpressionAST : ExpressionAST
  {
    protected NaryExpressionAST()
    {
      Operands = new List<ExpressionAST>();
    }

    protected NaryExpressionAST(int line, int column)
      : base(line, column)
    {
      Operands = new List<ExpressionAST>();
    }

    protected NaryExpressionAST(IEnumerable<ExpressionAST> operands, int line, int column)
      : base(line, column)
    {
      Operands = new List<ExpressionAST>();
      if (operands != null)
        Operands.AddRange(operands);
    }

    public List<ExpressionAST> Operands { get; private set; }
  }
}
