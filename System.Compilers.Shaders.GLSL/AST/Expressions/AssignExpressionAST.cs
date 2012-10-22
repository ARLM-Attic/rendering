using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Utils;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Expressions
{
  public class AssignExpressionAST : BinaryExpressionAST, IVarSetter
  {
    public AssignExpressionAST()
    {
    }

    public AssignExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public AssignExpressionAST(ExpressionAST lvalue, ExpressionAST expr, int line, int column)
      : base(lvalue, expr, line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
    {
      if (!LValue.IsLValue())
        context.Errors.Add(new SemanticError("The left-hand side of an assigment must be a lvalue", LValue.Line, LValue.Column));
      else
      {
        context.MarkErrors();
        LValue.CheckSemantic(context);
        if (!context.CheckForErrors())
        {
          LValueExpressionAST lvalue = LValue.Cast<LValueExpressionAST>();
          if (lvalue == null)
            context.Errors.Add(new SemanticError("The left-hand side of an assigment must be a lvalue", LValue.Line, LValue.Column));
          else if (!lvalue.IsWritable)
            context.Errors.Add(new SemanticError("The left-hand side of an assigment must be writable", LValue.Line, LValue.Column));

          context.MarkErrors();
          Expression.CheckSemantic(context);
          if (!context.CheckForErrors())
          {
            if (!LValue.Type.Equals(Expression.Type))
            {
              if (Expression.Type.ImplicitConvert(LValue.Type))
                context.Warnings.Add(new ImplicitConversionWarning(Expression.Type.Name, LValue.Type.Name, Expression.Line, Expression.Column));
              else
                context.Errors.Add(new CannotImplicitConvertError(Expression.Type.Name, LValue.Type.Name, Expression.Line, Expression.Column));
            }
          }
          context.UnMarkErrors();
          Type = lvalue.Type;
        }
        context.UnMarkErrors();
      }
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      return null;
    }

    public override bool IsConstant
    {
      get { return false; }
    }

    public override string GetOperatorToken()
    {
      return "=";
    }

    public List<VariableInfo> GetVarsSetted()
    {
      List<VariableInfo> varsSetted = new List<VariableInfo>();

      if (Expression.Is<IVarSetter>())
        varsSetted.AddRange(Expression.Cast<IVarSetter>().GetVarsSetted());
      
      if(LValue.Is<LocalVariableExpressionAST>())
        varsSetted.Add(LValue.Cast<LocalVariableExpressionAST>().VarInfo);
      
      return varsSetted;
    }

    public ExpressionAST LValue 
    {
      get { return FirstOp; }
      set { FirstOp = value; }
    }

    public ExpressionAST Expression 
    {
      get { return SecondOp; }
      set { SecondOp = value; }
    }

    
  }
}
