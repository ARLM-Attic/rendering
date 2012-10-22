using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Statements
{
  public class BreakStatementAST : ControlStatementAST
  {
    public BreakStatementAST()
    {
    }

    public BreakStatementAST(int line, int column)
      : base(line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
    {
      if (!context.Scope.AllowBreak)
        context.Errors.Add(new StatementNotAllowedError("break", Line, Column));
      AlwaysReturn = false;
    }

    public override List<VariableInfo> GetVarsSetted()
    {
      return Enumerable.Empty<VariableInfo>().ToList();
    }
  }
}
