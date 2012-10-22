using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Declarations;
using GLSLCompiler.AST.Expressions;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Statements
{
  public class ForControlStatementAST : ControlStatementAST
  {
    public ForControlStatementAST()
    {
    }

    public ForControlStatementAST(int line, int column)
      : base(line, column)
    {
    }

    public ForControlStatementAST(BaseAST init, ExpressionAST cond, ExpressionAST increment, StatementAST body, int line, int column)
      : base(line, column)
    {
      Init = init;
      Condition = cond;
      Increment = increment;
      Body = body;
    }

    public override void CheckSemantic(SemanticContext context)
    {
      context.PushScope(ScopeType.NonBreakableScope);
      if (Init != null)
      {
        if(Init.Is<FunctionDeclarationAST>())
          context.Errors.Add(new SemanticError("Function declarations isn't allowed in this context", Line, Column));
        else
          Init.CheckSemantic(context);
      }
      if (Condition != null)
      {
        context.MarkErrors();
        Condition.CheckSemantic(context);
        if (!context.CheckForErrors())
        {
          if(!Condition.Type.IsBool())
            context.Errors.Add(new CannotImplicitConvertError(Condition.Type.Name, GLSLTypes.BoolType.Name, Line, Column));
        }
        context.UnMarkErrors();
      }
      if (Increment != null)
        Increment.CheckSemantic(context);

      AlwaysReturn = false;

      context.PushScope(ScopeType.BreakableScope);
      if (Body != null)
      {
        context.MarkErrors();
        Body.CheckSemantic(context);
        if (!context.CheckForErrors())
        {
          if (Condition.IsConstant)
          {
            bool cond = Condition.GetConstantValue<bool>();
            if (cond)
              AlwaysReturn = Body.AlwaysReturn;
          }
        }
        context.UnMarkErrors();
      }
      context.PopScope();

      context.PopScope();
    }

    public override List<VariableInfo> GetVarsSetted()
    {
      if (!Condition.IsConstant)
        return Enumerable.Empty<VariableInfo>().ToList();
      bool cond = Condition.GetConstantValue<bool>();
      return cond ? Body.GetVarsSetted() : Enumerable.Empty<VariableInfo>().ToList();
    }

    public BaseAST Init { get; set; }

    public ExpressionAST Condition { get; set; }

    public ExpressionAST Increment { get; set; }

    public StatementAST Body { get; set; }

  }
}
