using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public abstract class ConstantExpressionAST<T> : ExpressionAST
  {
    protected ConstantExpressionAST()
    {
    }

    protected ConstantExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    protected ConstantExpressionAST(T value, int line, int column)
      : base(line, column)
    {
      Value = value;
    }

    public override bool IsConstant
    {
      get { return true; }
    }

    public T Value { get; set; }
  }
}
