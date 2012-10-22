using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Expressions
{
  public class ArrayConstructionAST : ConstructionExpressionAST
  {
    public ArrayConstructionAST()
    {
    }

    public ArrayConstructionAST(int line, int column)
      : base(line, column)
    {
    }

    public ArrayConstructionAST(string name, ExpressionAST expr, IEnumerable<ExpressionAST> args, int line, int column)
      : base(name, args, line, column)
    {
      Expression = expr;
    }

    public override void CheckSemantic(SemanticContext context)
    {
      if (Expression == null)
        CheckWithoutSize(context);
      else
        CheckWithSize(context);
    }

    private void CheckWithoutSize(SemanticContext context)
    {
      if (!context.Scope.ContainsType(Name))
        context.Errors.Add(new TypeNotDeclaredError(Name, Line, Column));
      else
      {
        int argCount = Arguments.Count;
        string newTypeName = ArrayType.GetArrayTypeName(Name, argCount);
        GLSLType elementType = context.Scope.GetTypeInfo(Name).Type;
        TypeInfo tInfo;
        if (!context.Scope.TryGetTypeInfo(newTypeName, out tInfo))
        {
          ArrayType newType = new ArrayType(newTypeName, elementType, argCount);
          GLSLTypes.RegisterType(newType);
          tInfo = new TypeInfo() { Name = newTypeName, Type = newType };
          context.Scope.AddType(tInfo);
        }

        foreach (var arg in Arguments)
        {
          context.MarkErrors();
          arg.CheckSemantic(context);
          if (!context.CheckForErrors())
          {
            if (!arg.Type.Equals(elementType))
            {
              if (arg.Type.ImplicitConvert(elementType))
                context.Warnings.Add(new ImplicitConversionWarning(arg.Type.Name, elementType.Name, arg.Line, arg.Column));
              else
                context.Errors.Add(new CannotImplicitConvertError(arg.Type.Name, elementType.Name, arg.Line, arg.Column));
            }
          }
          context.UnMarkErrors();
        }
        Type = tInfo.Type;
      }
    }

    private void CheckWithSize(SemanticContext context)
    {
      if (!context.Scope.ContainsType(Name))
        context.Errors.Add(new TypeNotDeclaredError(Name, Line, Column));
      else
      {
        context.MarkErrors();
        Expression.CheckSemantic(context);
        if (!context.CheckForErrors())
        {
          if (!Expression.IsConstant || !Expression.Type.IsInteger())
            context.Errors.Add(new SemanticError("Size expression of an array must be a integer constant value", Expression.Line, Expression.Column));
          else
          {
            int arrSize = Expression.GetConstantValue<int>();
            if (arrSize < 0)
              context.Errors.Add(new SemanticError("Size expression of an array must be greater than zero", Expression.Line, Expression.Column));

            GLSLType elementType = context.Scope.GetTypeInfo(Name).Type;
            string newTypeName = ArrayType.GetArrayTypeName(Name, arrSize);
            TypeInfo tInfo;
            if (!context.Scope.TryGetTypeInfo(newTypeName, out tInfo))
            {
              ArrayType newType = new ArrayType(newTypeName, elementType, arrSize);
              GLSLTypes.RegisterType(newType);
              tInfo = new TypeInfo() { Name = newTypeName, Type = newType };
              context.Scope.AddType(tInfo);
            }
            else
            {
              if (Arguments.Count != arrSize)
                context.Errors.Add(new InvalidArgumentsError(tInfo.Type.Name, Line, Column));

              foreach (var arg in Arguments)
              {
                context.MarkErrors();
                arg.CheckSemantic(context);
                if (!context.CheckForErrors())
                {
                  if (!arg.Type.Equals(elementType))
                  {
                    if (arg.Type.ImplicitConvert(elementType))
                      context.Warnings.Add(new ImplicitConversionWarning(arg.Type.Name, elementType.Name, arg.Line, arg.Column));
                    else
                      context.Errors.Add(new CannotImplicitConvertError(arg.Type.Name, elementType.Name, arg.Line, arg.Column));
                  }
                }
                context.UnMarkErrors();
              }
            }
            Type = tInfo.Type;
          }
        }
        context.UnMarkErrors();
      }
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      return new ArrayTypeInstance()
      {
        Values = Arguments.Select(arg => arg.GetConstantValue()).ToList()
      };
    }

    public ExpressionAST Expression { get; set; }
    
  }
}
