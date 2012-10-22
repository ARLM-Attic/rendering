using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Utils;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public abstract class ConstructionExpressionAST : FunctionCallExpressionAST
  {
    protected ConstructionExpressionAST()
    {
    }

    protected ConstructionExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    protected ConstructionExpressionAST(string name, IEnumerable<ExpressionAST> args, int line, int column)
      : base(name, args, line, column)
    {
    }

    public abstract override void CheckSemantic(SemanticContext context);

    protected void CheckArguments(SemanticContext context)
    {
      foreach (var arg in Arguments)
        arg.CheckSemantic(context);
    }

    internal abstract override TypeInstance GetConstantValueInternal();

    public override bool IsConstant
    {
      get { return Arguments.All(arg => arg.IsConstant); }
    }
  }
}
