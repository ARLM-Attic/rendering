using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Expressions;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Statements
{
  public class ExpressionStatementAST : StatementAST
  {
    public ExpressionStatementAST()
    {
    }

    public ExpressionStatementAST(int line, int column)
      : base(line, column)
    {
    }

    public ExpressionStatementAST(ExpressionAST expression, int line, int column)
      : base(line, column)
    {
      Expression = expression;
    }

    public override void CheckSemantic(SemanticContext context)
    {
      Expression.CheckSemantic(context);
      AlwaysReturn = false;
    }

    public override System.Collections.Generic.List<VariableInfo> GetVarsSetted()
    {
      if (Expression.Is<IVarSetter>())
        return Expression.Cast<IVarSetter>().GetVarsSetted();
      
      return Enumerable.Empty<VariableInfo>().ToList();
    }

    public ExpressionAST Expression { get; set; }
  }
}
