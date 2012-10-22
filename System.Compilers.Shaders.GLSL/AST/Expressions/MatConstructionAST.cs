using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Expressions
{
  public class MatConstructionAST : ConstructionExpressionAST
  {
    public MatConstructionAST()
    {
    }

    public MatConstructionAST(int line, int column)
      : base(line, column)
    {
    }

    public MatConstructionAST(string name, IEnumerable<ExpressionAST> args, int line, int column)
      : base(name, args, line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
    {
      TypeInfo tInfo;
      if (!context.Scope.TryGetTypeInfo(Name, out tInfo))
        context.Errors.Add(new TypeNotDeclaredError(Name, Line, Column));
      else
      {
        MatType matType = tInfo.Type.Cast<MatType>();
        if (matType == null)
          context.Errors.Add(new TypeNotDeclaredError(Name, Line, Column));
        else
        {
          context.MarkErrors();
          CheckArguments(context);
          if (!context.CheckForErrors())
          {
            if (Arguments.Count == 0)
              context.Errors.Add(new NoEnoughArgumentsError(matType.Name, Line, Column));
            else if ((Arguments.Count == 1 && !Arguments[0].Type.IsScalar()) || Arguments.Count > 1)
            {
              // check for a first argument matrix
              var firstArg = Arguments.First();
              if (firstArg.Type.IsMat())
              {
                if (Arguments.Count > 1)
                  context.Errors.Add(new SemanticError("Cannot have any other arguments for a matrix construction when the first argument is a matrix.", Line, Column));
              }
              else
              {
                int consumedLeft = matType.Size;
                foreach (var arg in Arguments)
                {
                  if (consumedLeft <= 0)
                    context.Errors.Add(new TooManyArgumentsError(matType.Name, Line, Column));
                  else if (arg.Type.IsSampler() || arg.Type.IsArray() || arg.Type.IsStruct())
                    context.Errors.Add(new InvalidArgumentsError(matType.Name, Line, Column));
                  else
                    consumedLeft -= arg.Type.GetNumberOfComponents();
                }
                if (consumedLeft > 0)
                  context.Errors.Add(new NoEnoughArgumentsError(matType.Name, Line, Column));
              }
            }
          }
        }
        Type = matType;
      }
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      MatType matType = GLSLTypes.GetTypeByName(Name).Cast<MatType>();
      
      List<TypeInstance> values = new List<TypeInstance>();

      if (Arguments.Count == 1 && Arguments[0].Type.IsScalar())
      {
        var fTypeInstance = Arguments[0].GetConstantValue();
        for (int j = 0; j < matType.Columns; j++)
        {
          for (int i = 0; i < matType.Rows; i++)
          {
            int pos = j * matType.Rows + i;
            if (i == j)
              values.Add(fTypeInstance);
            else
              values.Add(new FloatTypeInstance() { Value = 0.0 });
          }
        }
      }
      else if (Arguments[0].Type.IsMat())
      {
        MatType fMatType = Arguments[0].Type.Cast<MatType>();
        MatTypeInstance matInstance = Arguments[0].GetConstantValue().Cast<MatTypeInstance>();
        for (int j = 0; j < matType.Columns; j++)
        {
          for (int i = 0; i < matType.Rows; i++)
          {
            int pos = j * fMatType.Rows + i;
            if (pos < matInstance.Values.Count)
              values.Add(matInstance[pos]);
            else if (i == j)
              values.Add(new FloatTypeInstance() { Value = 1.0 });
            else
              values.Add(new FloatTypeInstance() { Value = 0.0 });
          }
        }
      }
      else
      {
        int components = matType.GetNumberOfComponents();
        values.AddRange(Arguments.SelectMany(arg => arg.GetConstantValue().Flat()).Take(components));
      }
      return new MatTypeInstance()
      {
        Values = values
      };
    }
  }
}
