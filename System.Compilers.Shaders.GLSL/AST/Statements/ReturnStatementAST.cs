using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.AST.Expressions;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Statements
{
  public class ReturnStatementAST : ControlStatementAST
  {
    public ReturnStatementAST()
    {
    }

    public ReturnStatementAST(int line, int column)
      : base(line, column)
    {
    }

    public ReturnStatementAST(ExpressionAST expression, int line, int column)
      : base(line, column)
    {
      Expression = expression;
    }
    
    public override void CheckSemantic(SemanticContext context)
    {
      if (!context.Scope.AllowReturn)
        context.Errors.Add(new StatementNotAllowedError("return", Line, Column));
      else
      {
        if(context.FuncInfo != null)
        {
          if (Expression != null)
          {
            context.MarkErrors();
            Expression.CheckSemantic(context);
            if (!context.CheckForErrors())
            {
              if (!Expression.Type.Equals(context.FuncInfo.ReturnType))
              {
                if (Expression.Type.ImplicitConvert(context.FuncInfo.ReturnType))
                  context.Warnings.Add(new ImplicitConversionWarning(Expression.Type.Name, context.FuncInfo.ReturnType.Name, Line, Column));
                else
                  context.Errors.Add(new CannotImplicitConvertError(Expression.Type.Name, context.FuncInfo.ReturnType.Name, Line, Column));
              }
            }
            context.UnMarkErrors();
          }
          else
          {
            if (!context.FuncInfo.ReturnType.IsVoid())
              context.Errors.Add(new StatementNotAllowedError("return", Line, Column));
          }
        }
        AlwaysReturn = true;
      }
    }

    public override List<VariableInfo> GetVarsSetted()
    {
      return Expression.Is<IVarSetter>() ? 
              Expression.Cast<IVarSetter>().GetVarsSetted() : 
              Enumerable.Empty<VariableInfo>().ToList();
    }

    public ExpressionAST Expression { get; set; }
  }
}
