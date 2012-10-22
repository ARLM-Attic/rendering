using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Expressions
{
  public class VecConstructionAST : ConstructionExpressionAST
  {
    public VecConstructionAST()
    {
    }

    public VecConstructionAST(int line, int column)
      : base(line, column)
    {
    }

    public VecConstructionAST(string name, IEnumerable<ExpressionAST> args, int line, int column)
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
        VecType vecType = tInfo.Type.Cast<VecType>();
        if (vecType == null)
          context.Errors.Add(new TypeNotDeclaredError(Name, Line, Column));
        else
        {
          context.MarkErrors();
          CheckArguments(context);
          if (!context.CheckForErrors())
          {
            if (Arguments.Count == 0)
              context.Errors.Add(new NoEnoughArgumentsError(vecType.Name, Line, Column));
            else if ((Arguments.Count == 1 && !Arguments[0].Type.IsScalar()) || Arguments.Count > 1)
            {
              int consumedLeft = vecType.Size;
              foreach (var arg in Arguments)
              {
                if (consumedLeft <= 0)
                  context.Errors.Add(new TooManyArgumentsError(vecType.Name, Line, Column));
                else if (arg.Type.IsSampler() || arg.Type.IsArray() || arg.Type.IsStruct())
                  context.Errors.Add(new InvalidArgumentsError(vecType.Name, Line, Column));
                else
                  consumedLeft -= arg.Type.GetNumberOfComponents();
              }
              if (consumedLeft > 0)
                context.Errors.Add(new NoEnoughArgumentsError(vecType.Name, Line, Column));
            }
          }
          context.UnMarkErrors();
        }
        Type = vecType;
      }
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      GLSLType type = GLSLTypes.GetTypeByName(Name);
      int components = type.GetNumberOfComponents();

      var flattenArgs = Arguments.SelectMany(arg => arg.GetConstantValue().Flat());
      if (flattenArgs.Count() == 1)
      {
        return new VecTypeInstance()
        {
          Values = flattenArgs.First().NonSingleton(components).ToList()
        };
      }
      return new VecTypeInstance()
      {
        Values = flattenArgs.Take(components).ToList()
      };
    }
  }
}
