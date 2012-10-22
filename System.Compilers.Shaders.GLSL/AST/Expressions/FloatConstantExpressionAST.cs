using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public class FloatConstantExpressionAST : ConstantExpressionAST<float>
  {
    public FloatConstantExpressionAST()
    {
    }

    public FloatConstantExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public FloatConstantExpressionAST(float value, int line, int column)
      : base(value, line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
    {
      Type = GLSLTypes.FloatType;
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      return new FloatTypeInstance()
      {
        Value = Value
      };
    }
  }
}
