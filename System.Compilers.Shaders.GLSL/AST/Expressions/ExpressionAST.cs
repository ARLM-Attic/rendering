using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public abstract class ExpressionAST : BaseAST, ITypeable
  {
    protected ExpressionAST()
    {
    }

    protected ExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public T GetConstantValue<T>()
    {
      return (T)GetConstantValue().Value;
    }

    public TypeInstance GetConstantValue()
    {
      if (!IsConstant)
        throw new InvalidOperationException("Cannot retrieve constant value for non-constant expressions");
      return GetConstantValueInternal();
    }

    internal abstract TypeInstance GetConstantValueInternal();

    public abstract bool IsConstant { get; }

    public GLSLType Type { get; set; }
  }
}
