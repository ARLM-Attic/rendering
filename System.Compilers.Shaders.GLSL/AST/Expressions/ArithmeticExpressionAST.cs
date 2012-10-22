using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using System.Reflection.Emit;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Expressions
{
  public abstract class ArithmeticExpressionAST : BinaryExpressionAST
  {
    protected ArithmeticExpressionAST()
    {
    }

    protected ArithmeticExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    protected ArithmeticExpressionAST(ExpressionAST firstOp, ExpressionAST secondOp, int line, int column)
      : base(firstOp, secondOp, line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
    {
      context.MarkErrors();
      FirstOp.CheckSemantic(context);
      SecondOp.CheckSemantic(context);
      if (!context.CheckForErrors())
      {
        if (FirstOp.Type.IsFloatBased() && SecondOp.Type.IsIntegerBased())
        {
          GLSLType secondOpFloatType = SecondOp.Type.GetFloatBased();
          if(PerformTypeCheck(FirstOp.Type, secondOpFloatType))
            context.Warnings.Add(new ImplicitConversionWarning(SecondOp.Type.Name, secondOpFloatType.Name, SecondOp.Line, SecondOp.Column));
          else
            context.Errors.Add(new CannotAppliedOperatorError(GetOperatorToken(), FirstOp.Type.Name, SecondOp.Type.Name, FirstOp.Line, FirstOp.Column));
        }
        else if (FirstOp.Type.IsIntegerBased() && SecondOp.Type.IsFloatBased())
        {
          GLSLType firstOpFloatType = FirstOp.Type.GetFloatBased();
          if(PerformTypeCheck(firstOpFloatType, SecondOp.Type))
            context.Warnings.Add(new ImplicitConversionWarning(FirstOp.Type.Name, firstOpFloatType.Name, FirstOp.Line, FirstOp.Column));
          else
            context.Errors.Add(new CannotAppliedOperatorError(GetOperatorToken(), FirstOp.Type.Name, SecondOp.Type.Name, FirstOp.Line, FirstOp.Column));
        }
        else if((FirstOp.Type.IsFloatBased() && SecondOp.Type.IsFloatBased()) ||(FirstOp.Type.IsIntegerBased() && SecondOp.Type.IsIntegerBased()))
        {
          if(!PerformTypeCheck(FirstOp.Type, SecondOp.Type))
            context.Errors.Add(new CannotAppliedOperatorError(GetOperatorToken(), FirstOp.Type.Name, SecondOp.Type.Name, FirstOp.Line, FirstOp.Column));
        }
        else 
        {
          context.Errors.Add(new CannotAppliedOperatorError(GetOperatorToken(), FirstOp.Type.Name, SecondOp.Type.Name, FirstOp.Line, FirstOp.Column));
        }
      }
      context.UnMarkErrors();
    }

    private bool PerformTypeCheck(GLSLType type1, GLSLType type2)
    {
      GLSLType resultType = null;
      if (type1 != null && type2 != null && PerformTypeCheck(type1, type2, out resultType))
        Type = resultType;
      return Type != null;
    }

    private bool PerformTypeCheck(GLSLType type1, GLSLType type2, out GLSLType resultType)
    {
      resultType = null;
      if (type1.IsScalar() && type2.IsScalar())
        resultType = type1;
      else if (type1.IsScalar() && type2.IsVec())
        resultType = type2;
      else if (type1.IsVec() && type2.IsScalar())
        resultType = type1;
      else if (type1.IsScalar() && type2.IsMat())
        resultType = type2;
      else if (type1.IsMat() && type2.IsScalar())
        resultType = type1;
      else if (type1.IsVec() && type2.IsVec())
      {
        VecType vecType1 = (VecType)type1;
        VecType vecType2 = (VecType)type2;
        if (vecType1.Size == vecType2.Size)
          resultType = vecType1;
      }
      else if (type1.IsVec() && type2.IsMat())
      {
        if (this is MulExpressionAST)
        {
          VecType vecType1 = (VecType)type1;
          MatType matType2 = (MatType)type2;
          if (vecType1.Size == matType2.Rows)
            resultType = Helper.GetFloatVec(matType2.Columns);
        }
      }
      else if (type1.IsMat() && type2.IsVec())
      {
        if (this is MulExpressionAST)
        {
          MatType matType1 = (MatType)type1;
          VecType vecType2 = (VecType)type2;
          if (matType1.Columns == vecType2.Size)
            resultType = Helper.GetFloatVec(matType1.Rows);
        }
      }
      else if (type1.IsMat() && type2.IsMat())
      {
        MatType matType1 = (MatType)type1;
        MatType matType2 = (MatType)type2;
        if (this is MulExpressionAST)
        {
          if (matType1.Columns == matType2.Rows)
            resultType = Helper.GetMatType(matType1.Rows, matType2.Columns);
        }
        else if (matType1.Rows == matType2.Rows && matType1.Columns == matType2.Columns)
          resultType = matType1;
      }
      return resultType != null;
    }
  }
}
