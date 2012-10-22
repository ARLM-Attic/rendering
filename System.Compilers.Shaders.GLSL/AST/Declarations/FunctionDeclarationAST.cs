using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.AST.Statements;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Declarations
{
  public class FunctionDeclarationAST : DeclarationAST
  {
    public FunctionDeclarationAST()
    {
      Parameters = new List<ParameterDeclarationAST>();
    }

    public FunctionDeclarationAST(int line, int column)
      : base(line, column)
    {
      Parameters = new List<ParameterDeclarationAST>();
    }

    public FunctionDeclarationAST(string name, IEnumerable<ParameterDeclarationAST> parameters, int line, int column)
      : base(line, column)
    {
      Name = name;
      Parameters = new List<ParameterDeclarationAST>(parameters);
    }

    public override void CheckSemantic(SemanticContext context)
    {
      context.MarkErrors();
      TypeSpecifier.CheckSemantic(context);
      if (!context.CheckForErrors())
      {
        Scope prevScope = context.Scope;

        context.PushScope(ScopeType.FunctionDeclarationScope);

        context.MarkErrors();
        CheckParameters(context);

        if (!context.CheckForErrors())
        {
          List<FunctionInfo> fInfos;
          if (!prevScope.TryGetFunctionInfo(Name, out fInfos))
            fInfos = new List<FunctionInfo>();
          FunctionInfo fInfo = fInfos.FirstOrDefault(f => Parameters.Select(p => p.TypeSpecifier.Type).SequenceEqual(f.ParamInfos.Select(pInfo => pInfo.Type)));

          // Una funcion puede ser declarada multiples veces, pero solo puede ser definida una vez
          if (fInfo == null)
          {
            fInfo = new FunctionInfo()
            {
              Name = Name,
              ReturnType = ReturnType,
              IsDefined = false,
              IsContructor = false,
              ParamInfos = Parameters.Select(p => p.VarInfo.Cast<ParamInfo>()).ToList()
            };
            prevScope.AddFunction(fInfo);
          }
          FuncInfo = fInfo;
        }
        context.PopScope();
      }
      context.UnMarkErrors();
    }

    protected void CheckParameters(SemanticContext context)
    {
      bool defaultValue = false;
      foreach (var param in Parameters)
      {
        param.CheckSemantic(context);
        if(defaultValue && param.DefaultExpression == null)
          context.Errors.Add(new SemanticError("Parameters with default values must be the last ones", param.Line, param.Column));
        else if (param.DefaultExpression != null)
          defaultValue = true;
      }
    }

    public string Name { get; set; }

    public GLSLType ReturnType
    {
      get { return TypeSpecifier.Type; }
      set { TypeSpecifier.Type = value; }
    }

    public FunctionInfo FuncInfo { get; set; }

    public List<ParameterDeclarationAST> Parameters { get; private set; }

  }
}
