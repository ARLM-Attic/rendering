using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public class IntegerConstantExpressionAST : ConstantExpressionAST<int>
  {
    public IntegerConstantExpressionAST()
    {
    }

    public IntegerConstantExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public IntegerConstantExpressionAST(int value, int line, int column)
      : base(value, line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
    {
      Type = GLSLTypes.IntegerType;
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      return new IntTypeInstance()
      {
        Value = Value
      };
    }
  }
}
