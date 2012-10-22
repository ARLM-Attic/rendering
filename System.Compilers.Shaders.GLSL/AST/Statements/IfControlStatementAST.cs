using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Expressions;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Statements
{
  public class IfControlStatementAST : ControlStatementAST
  {
    public IfControlStatementAST()
    {
    }

    public IfControlStatementAST(int line, int column)
      : base(line, column)
    {
    }

    public IfControlStatementAST(ExpressionAST condition, StatementAST ontrue, StatementAST onfalse, int line, int column)
      : base(line, column)
    {
      Condition = condition;
      OnTrue = ontrue;
      OnFalse = onfalse;
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

      if (OnTrue.Is<DeclarationStatementAST>())
        context.Errors.Add(new StatementNotAllowedError("Declaration", Line, Column));

      OnTrue.CheckSemantic(context);
      if (OnFalse == null)
        AlwaysReturn = false;
      else
      {
        if (OnFalse.Is<DeclarationStatementAST>())
          context.Errors.Add(new StatementNotAllowedError("Declaration", Line, Column));

        OnFalse.CheckSemantic(context);
        AlwaysReturn = OnTrue.AlwaysReturn && OnFalse.AlwaysReturn;
      }

      if (Condition.IsConstant)
      {
        bool cond = Condition.GetConstantValue<bool>();
        AlwaysReturn = cond ? OnTrue.AlwaysReturn : (OnFalse != null ? OnFalse.AlwaysReturn : false);
      }
    }

    public override List<VariableInfo> GetVarsSetted()
    {
      if (!Condition.IsConstant)
        return Enumerable.Empty<VariableInfo>().ToList();
      bool cond = Condition.GetConstantValue<bool>();
      if (cond)
        return OnTrue.GetVarsSetted();
      return OnFalse != null ? OnFalse.GetVarsSetted() : Enumerable.Empty<VariableInfo>().ToList(); 
    }

    public ExpressionAST Condition { get; set; }

    public StatementAST OnTrue { get; set; }

    public StatementAST OnFalse { get; set; }
  }
}
