using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Declarations;
using GLSLCompiler.AST.Expressions;
using GLSLCompiler.Types;
using System.Reflection.Emit;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Expressions
{
  public class FunctionCallExpressionAST : NaryExpressionAST
  {
    public FunctionCallExpressionAST()
    {
    }

    public FunctionCallExpressionAST(int line, int column)
      : base(line, column)
    {
    }

    public FunctionCallExpressionAST(string name, IEnumerable<ExpressionAST> arguments, int line, int column)
      : base(arguments, line, column)
    {
      Name = name;
    }

    public override void CheckSemantic(SemanticContext context)
    {
      List<FunctionInfo> fInfos;
      if (!context.Scope.TryGetFunctionInfo(Name, out fInfos))
        context.Errors.Add(new FunctionNotDeclaredError(Name, Line, Column));
      else
      {
        context.MarkErrors();
        foreach (var arg in Arguments)
          arg.CheckSemantic(context);

        if (!context.CheckForErrors())
        {
          List<FunctionInfo> exactMatch;
          List<FunctionInfo> overloadMatch;
          if (!GetOverloads(fInfos, out exactMatch, out overloadMatch))
            context.Errors.Add(new SemanticError("The best overloaded function match for '{0}' has some invalid arguments".Fmt(Name), Line, Column));
          else
          {
            if (exactMatch.Count > 0)
            {
              if (exactMatch.Count > 1)
                context.Errors.Add(new AmbiguityFunctionCallError(exactMatch.Select(fInfo => fInfo.GetSignature()), Line, Column));
              else
                FuncInfo = exactMatch.First();
            }
            else if (overloadMatch.Count > 0)
            {
              if (overloadMatch.Count > 1)
                context.Errors.Add(new AmbiguityFunctionCallError(overloadMatch.Select(fInfo => fInfo.GetSignature()), Line, Column));
              else
                FuncInfo = exactMatch.First();
            }

            // check arguments for implicit conversion warnings and for lvalues in out parameters
            for (int i = 0; i < FuncInfo.ParamInfos.Count; i++)
            {
              if (!Arguments[i].Type.Equals(FuncInfo.ParamInfos[i].Type))
              {
                if (Arguments[i].Type.ImplicitConvert(FuncInfo.ParamInfos[i].Type))
                  context.Warnings.Add(new ImplicitConversionWarning(Arguments[i].Type.Name, FuncInfo.ParamInfos[i].Type.Name, Arguments[i].Line, Arguments[i].Column));
              }
              if (FuncInfo.ParamInfos[i].Qualifier == ParamQualifier.Out ||
                FuncInfo.ParamInfos[i].Qualifier == ParamQualifier.InOut)
              {
                if (!Arguments[i].IsLValue() || !Arguments[i].Cast<LValueExpressionAST>().IsWritable)
                  context.Errors.Add(new SemanticError("A writable lvalue is required for 'inout' or 'out' parameters", Arguments[i].Line, Arguments[i].Column));
              }
            }
            Type = FuncInfo.ReturnType;

            if (FuncInfo != null && !context.FunctionsCalled.Contains(FuncInfo))
              context.FunctionsCalled.Add(FuncInfo);
          }
        }
        context.UnMarkErrors();
      }
    }

    private bool GetOverloads(IEnumerable<FunctionInfo> fInfos, out List<FunctionInfo> exactMatchs, out List<FunctionInfo> overloadMatchs)
    {
      exactMatchs = new List<FunctionInfo>();
      overloadMatchs = new List<FunctionInfo>();
      foreach (var fInfo in fInfos)
      {
        if (Arguments.Count <= fInfo.ParamInfos.Count)
        {
          if (!fInfo.ParamInfos.Skip(Arguments.Count).All(pInfo => pInfo.HasDefaultValue))
            continue;
          
          var argsTypes = Arguments.Select(arg => arg.Type);
          var pInfosTypes = fInfo.ParamInfos.Select(pInfo => pInfo.Type);
          var pairTypes = argsTypes.Compact(pInfosTypes, (t1, t2) => new { Type1 = t1, Type2 = t2 });

          bool exact = true;
          bool overload = false;
          foreach (var pair in pairTypes)
          {
            if (!exact && !overload)
              break;
            if (!pair.Type1.Equals(pair.Type2))
            {
              exact = false;
              if (pair.Type1.ImplicitConvert(pair.Type2))
                overload = true;
            }
          }
          if (exact)
            exactMatchs.Add(fInfo);
          else if (overload)
            overloadMatchs.Add(fInfo);
        }
      }
      return exactMatchs.Count > 0 || overloadMatchs.Count > 0;
    }

    internal override TypeInstance GetConstantValueInternal()
    {
      if (FuncInfo.IsContructor)
      {
        StructType sType = GLSLTypes.GetTypeByName(Name).Cast<StructType>();
        return new StructTypeInstance()
        {
          Values = Arguments.Select((arg, i) => new StructTypeInstance.FieldInstance() 
          { 
            FieldName = sType.FieldsInfo[i].Name, 
            Value = arg.GetConstantValue() 
          }).ToList()
        };
      }
      throw new InvalidOperationException();
    }

    public override bool IsConstant
    {
      get
      {
        // aqui hay que chequear si la funcion es built-in, porque
        // se permite llamar a las funciones built-in con parametros constantes
        // habria que llamar a la funcion en tiempo de compilacion para tener su resultado
        return false;
      }
    }

    public string Name { get; set; }

    public FunctionInfo FuncInfo { get; set; }

    public List<ExpressionAST> Arguments 
    {
      get { return Operands; }
    }
  
  }
}
