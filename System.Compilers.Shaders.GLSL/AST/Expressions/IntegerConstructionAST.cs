using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Expressions
{
  class IntegerConstructionAST : ConstructionExpressionAST
  {
    public IntegerConstructionAST()
    {
    }

    public IntegerConstructionAST(int line, int column)
      : base(line, column)
    {
    }

    public IntegerConstructionAST(string name, IEnumerable<ExpressionAST> args, int line, int column)
      : base(name, args, line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
    {
      if (Arguments.Count == 0)
        context.Errors.Add(new NoEnoughArgumentsError(Name, Line, Column));
      else if (Arguments.Count > 1)
        context.Errors.Add(new TooManyArgumentsError(Name, Line, Column));
      else
      {
        var arg = Arguments[0];

        context.MarkErrors();
        arg.CheckSemantic(context);

        if (!context.CheckForErrors())
        {
          var flattenArg = arg.Type.GetFlatTypes();
          GLSLType first = flattenArg.FirstOrDefault();
          if (first != null && (first.IsSampler() || first.IsArray() || first.IsStruct()))
            context.Errors.Add(new InvalidArgumentsError(Name, Line, Column));
        }
        context.UnMarkErrors();
      }
      Type = GLSLTypes.IntegerType;
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      if (Arguments.Count == 0)
        throw new InvalidOperationException();
      IEnumerable<TypeInstance> tInstances = Arguments[0].GetConstantValue().Flat();
      if(!tInstances.Any())
        throw new InvalidOperationException();
      return new IntTypeInstance()
      {
        Value = tInstances.First().Value
      };
    }
  }
}
