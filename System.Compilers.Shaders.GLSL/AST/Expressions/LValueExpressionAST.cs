using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Declarations;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public abstract class LValueExpressionAST : ExpressionAST
  {
    protected LValueExpressionAST()
    {
    }

    protected LValueExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    protected LValueExpressionAST(string name, int line, int column)
      : base(line, column)
    {
      Name = name;
    }

    public string Name { get; set; }

    public abstract bool IsWritable { get; }
  }
}
