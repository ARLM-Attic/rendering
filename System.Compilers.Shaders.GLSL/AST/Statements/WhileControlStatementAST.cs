using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Expressions;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Statements
{
  public class WhileControlStatementAST : ControlStatementAST
  {
    public WhileControlStatementAST()
    {
    }

    public WhileControlStatementAST(int line, int column)
      : base(line, column)
    {
    }

    public WhileControlStatementAST(ExpressionAST condition, StatementAST body, int line, int column)
      : base(line, column)
    {
      Condition = condition;
      Body = body;
    }

    public override void CheckSemantic(SemanticContext context)
    {
      context.MarkErrors();
      Condition.CheckSemantic(context);
      if (!context.CheckForErrors())
      {
        if (!Condition.Type.IsBool())
          context.Errors.Add(new CannotImplicitConvertError(Condition.Type.Name, GLSLTypes.BoolType.Name, Line, Column));
      }
      context.UnMarkErrors();

      if (Body.Is<DeclarationStatementAST>())
        context.Errors.Add(new StatementNotAllowedError("Declaration", Body.Line, Body.Column));
      else
      {
        context.PushScope(ScopeType.BreakableScope);
        Body.CheckSemantic(context);
        context.PopScope();
      }

      AlwaysReturn = false;
      if (Condition.IsConstant)
      {
        bool cond = Condition.GetConstantValue<bool>();
        if (cond)
          AlwaysReturn = Body.AlwaysReturn;
      }
    }

    public override List<VariableInfo> GetVarsSetted()
    {
      if (!Condition.IsConstant)
        return Enumerable.Empty<VariableInfo>().ToList();
      bool cond = Condition.GetConstantValue<bool>();
      return cond ? Body.GetVarsSetted() : Enumerable.Empty<VariableInfo>().ToList();
    }

    public ExpressionAST Condition { get; set; }

    public StatementAST Body { get; set; }
  }
}
