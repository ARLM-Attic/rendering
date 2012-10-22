using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;
using GLSLCompiler.AST.Expressions;

namespace GLSLCompiler.AST.Statements
{
  public class DoWhileControlStatementAST : ControlStatementAST
  {
    public DoWhileControlStatementAST()
    {
    }

    public DoWhileControlStatementAST(int line, int column)
      : base(line, column)
    {
    }

    public DoWhileControlStatementAST(ExpressionAST condition, StatementAST body, int line, int column)
      : base(line, column)
    {
      Condition = condition;
      Body = body;
    }

    public override void CheckSemantic(SemanticContext context)
    {
      context.PushScope(ScopeType.BreakableScope);
      Body.CheckSemantic(context);
      context.PopScope();

      AlwaysReturn = Body.AlwaysReturn;

      context.MarkErrors();
      Condition.CheckSemantic(context);
      if (!context.CheckForErrors())
      {
        if (!Condition.Type.IsBool())
          context.Errors.Add(new CannotImplicitConvertError(Condition.Type.Name, GLSLTypes.BoolType.Name, Line, Column));
      }
      context.UnMarkErrors();
    }

    public override List<VariableInfo> GetVarsSetted()
    {
      return Body.GetVarsSetted();
    }

    public ExpressionAST Condition { get; set; }

    public StatementAST Body { get; set; }
  }
}
