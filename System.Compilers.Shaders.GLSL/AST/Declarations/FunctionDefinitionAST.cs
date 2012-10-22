using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Statements;
using GLSLCompiler.Utils;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Declarations
{
  public class FunctionDefinitionAST : FunctionDeclarationAST
  {
    public FunctionDefinitionAST()
    {
    }

    public FunctionDefinitionAST(int line, int column)
      : base(line, column)
    {
    }

    public FunctionDefinitionAST(string name, IEnumerable<ParameterDeclarationAST> parameters, CompoundStatementAST body, int line, int column)
      : base(name, parameters, line, column)
    {
      Body = body;
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

          if (fInfo != null)
          {
            if (fInfo.IsDefined)
              context.Errors.Add(new FunctionRedefinitionError(Name, Line, Column));
            else
              fInfo.IsDefined = true;
          }
          else
          {
            fInfo = new FunctionInfo()
            {
              Name = Name,
              ReturnType = ReturnType,
              IsDefined = true,
              IsContructor = false,
              IsBuiltIn = false,
              ParamInfos = Parameters.Select(p => p.VarInfo.Cast<ParamInfo>()).ToList()
            };
          }
          prevScope.AddFunction(fInfo);
          context.FuncInfo = fInfo;       // para que el 'return' pueda acceder al tipo de retorno de la funcion
          FuncInfo = fInfo;
        }
        CheckBody(context);
        context.FuncInfo = null;          // resetearlo para que no haya lio con otras funciones

        context.PopScope();
      }
      context.UnMarkErrors();
    }

    private void CheckBody(SemanticContext context)
    {
      context.MarkErrors();
      Body.CheckSemantic(context);
      if (!context.CheckForErrors())
      {
        CheckForNotAllPathReturn(context);
        CheckParametersOutSetted(context);
        CheckForUnreachableCode(context);
      }
      context.UnMarkErrors();
    }

    private void CheckForNotAllPathReturn(SemanticContext context)
    {
      if (!ReturnType.Equals(GLSLTypes.VoidType) && !Body.AlwaysReturn)
        context.Errors.Add(new NotAllPathReturnError(Name, Line, Column));
    }

    private void CheckForUnreachableCode(SemanticContext context)
    {
      bool alwaysReturn = false;
      foreach (var stmt in Body.Statements)
      {
        if (alwaysReturn)
        {
          context.Warnings.Add(new CompilerWarning("Unreachable code detected", stmt.Line, stmt.Column));
          break;
        }
        alwaysReturn = stmt.AlwaysReturn;
      }
    }

    private void CheckParametersOutSetted(SemanticContext context)
    {
      List<VariableInfo> varsSetted = Body.GetVarsSetted();
      foreach (var paramDec in Parameters.Where(p => p.Qualifier == ParamQualifier.Out || p.Qualifier == ParamQualifier.InOut))
      {
        bool setted = false;
        foreach (var vInfo in varsSetted.Where(vInfo => vInfo.Is<ParamInfo>()))
        {
          if (vInfo.Name == paramDec.Name && vInfo.Type.Equals(paramDec.TypeSpecifier.Type))
          {
            setted = true;
            break;
          }
        }
        if (!setted)
          context.Errors.Add(new SemanticError(String.Format("The out parameter '{0}' must be assigned to before control leaves the current function", paramDec.Name), paramDec.Line, paramDec.Column));
      }
    }

    public CompoundStatementAST Body { get; set; }
  }
}
