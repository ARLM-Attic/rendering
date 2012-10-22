using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Declarations;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Expressions
{
  public class ArrayExpressionAST : LValueExpressionAST
  {
    public ArrayExpressionAST()
    {
    }

    public ArrayExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public ArrayExpressionAST(ExpressionAST lvalue, ExpressionAST expr, int line, int column)
      : base("", line, column)
    {
      LValue = lvalue;
      Expression = expr;
    }

    public override void CheckSemantic(SemanticContext context)
    {
      context.MarkErrors();
      LValue.CheckSemantic(context);
      if (!context.CheckForErrors())
      {
        LValueExpressionAST lvalue = LValue.Cast<LValueExpressionAST>();
        if (lvalue == null || (!lvalue.Type.IsArray() && !lvalue.Type.IsMat() && !lvalue.Type.IsVec()))
          context.Errors.Add(new CannotApplyIndexingError(LValue.Type.Name, Line, Column));
        else
        {
          ArrayType arrType = lvalue.Type.Cast<ArrayType>();
          if (arrType.Size == null)
            context.Errors.Add(new SemanticError("Cannot apply indexing with '[]' to an array with undefined size", Line, Column));
          
          context.MarkErrors();
          Expression.CheckSemantic(context);
          if (!context.CheckForErrors())
          {
            if (!Expression.Type.IsInteger())
              context.Errors.Add(new CannotImplicitConvertError(Expression.Type.Name, GLSLTypes.IntegerType.Name, Expression.Line, Expression.Column));
          }
          context.UnMarkErrors();

          if (Expression.IsConstant)
          {
            int value = Expression.GetConstantValue<int>();
            if (!arrType.IsInRange(value))
              context.Errors.Add(new IndexOutOfRangeError(0, arrType.GetLength(0), Expression.Line, Expression.Column));
          }
          if (arrType.Is<MatType>())
            Type = Helper.GetFloatVec(arrType.Cast<MatType>().Columns);
          else
            Type = arrType.ElementType;
        }
      }
      context.UnMarkErrors();
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      TypeInstance lvalueConstValue = LValue.GetConstantValue();
      int index = Expression.GetConstantValue<int>();
      return lvalueConstValue[index];
    }

    public override bool IsConstant
    {
      get { return LValue.IsConstant; }
    }

    public ExpressionAST LValue { get; set; }

    public ExpressionAST Expression { get; set; }

    public override bool IsWritable
    {
      get
      {
        return LValue.IsLValue() ? LValue.Cast<LValueExpressionAST>().IsWritable : false;
      }
    }

  }
}
