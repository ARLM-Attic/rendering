using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.AST.Statements
{
  public class ContinueStatementAST : ControlStatementAST
  {
    public ContinueStatementAST()
    {
    }

    public ContinueStatementAST(int line, int column)
      : base(line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
    {
      if (!context.Scope.AllowContinue)
        context.Errors.Add(new StatementNotAllowedError("continue", Line, Column));
      AlwaysReturn = false;
    }

    public override List<VariableInfo> GetVarsSetted()
    {
      return Enumerable.Empty<VariableInfo>().ToList();
    }
  }
}
