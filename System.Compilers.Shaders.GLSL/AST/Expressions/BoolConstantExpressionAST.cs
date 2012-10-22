using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public class BoolConstantExpressionAST : ConstantExpressionAST<bool>
  {
    public BoolConstantExpressionAST()
    {
    }

    public BoolConstantExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public BoolConstantExpressionAST(bool value, int line, int column)
      : base(value, line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
    {
      Type = GLSLTypes.BoolType;
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      return new BoolTypeInstance()
      {
        Value = Value
      };
    }
  }
}
