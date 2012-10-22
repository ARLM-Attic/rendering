using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Statements
{
  public class CompoundStatementAST : StatementAST
  {
    public CompoundStatementAST()
    {
      Statements = new List<StatementAST>();
    }

    public CompoundStatementAST(int line, int column)
      : base(line, column)
    {
      Statements = new List<StatementAST>();
    }

    public CompoundStatementAST(IEnumerable<StatementAST> statements, int line, int column)
      : base(line, column)
    {
      Statements = new List<StatementAST>(statements);
    }

    public CompoundStatementAST(IEnumerable<StatementAST> statements, bool newScope, int line, int column)
      : base(line, column)
    {
      Statements = new List<StatementAST>(statements);
      NewScope = newScope;
    }

    public override void CheckSemantic(SemanticContext context)
    {
      if (NewScope)
        context.PushScope(ScopeType.NonBreakableScope);
      foreach (var stmt in Statements)
      {
        stmt.CheckSemantic(context);
        if (stmt.AlwaysReturn)
          AlwaysReturn = true;
      }
      if (NewScope)
        context.PopScope();
    }

    public override List<VariableInfo> GetVarsSetted()
    {
      List<VariableInfo> varsSetted = new List<VariableInfo>();
      foreach (var stmt in Statements)
        varsSetted.AddRange(stmt.GetVarsSetted());
      return varsSetted;
    }

    public List<StatementAST> Statements { get; private set; }
  }
}
