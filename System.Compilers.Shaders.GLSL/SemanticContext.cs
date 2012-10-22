using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Utils;

namespace GLSLCompiler
{
  public class SemanticContext
  {
    private Stack<int> errorsStack;

    public SemanticContext()
      : this(null)
    {
    }

    public SemanticContext(Scope scope)
    {
      errorsStack = new Stack<int>();
      Scope = scope;
      Errors = new List<CompilerError>();
      Warnings = new List<CompilerWarning>();
      FunctionsCalled = new List<FunctionInfo>();
    }

    public void MarkErrors()
    {
      errorsStack.Push(Errors.Count);
    }

    public void UnMarkErrors()
    {
      if(errorsStack.Count > 0)
        errorsStack.Pop();
    }

    public bool CheckForErrors()
    {
      int prevErrorsCount = 0;
      if(errorsStack.Count > 0)
        prevErrorsCount = errorsStack.Peek();
      return Errors.Count > prevErrorsCount;
    }

    public void PushScope(ScopeType scopeType)
    {
      Scope = new Scope(scopeType, Scope);
    }

    public void PopScope()
    {
      Scope = Scope.ParentScope;
    }

    public List<CompilerError> Errors { get; set; }

    public List<CompilerWarning> Warnings { get; set; }

    public Scope Scope { get; set; }

    public ShaderType ShaderType { get; set; }

    internal FunctionInfo FuncInfo { get; set; }

    internal List<FunctionInfo> FunctionsCalled { get; private set; }

    public bool AllowDiscard
    {
      get { return ShaderType == ShaderType.FramentShader; }
    }
  }

  public enum ShaderType
  {
    VertexShader,
    FramentShader,
    GeometryShader
  }
}
  