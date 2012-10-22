using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Declarations;
using GLSLCompiler.Utils;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST
{
  public class ShaderAST : BaseAST
  {
    public ShaderAST()
      : this(Enumerable.Empty<DeclarationAST>())
    {
    }

    public ShaderAST(IEnumerable<DeclarationAST> decls)
      : base(0, 0)
    {
      Declarations = new List<DeclarationAST>(decls);
    }

    public override void CheckSemantic(SemanticContext context)
    {
      foreach (var dec in Declarations)
        dec.CheckSemantic(context);
      
      if(!context.Scope.ContainsFunction("main", new GLSLType[0]))
        context.Errors.Add(new NoMainFunctionDefinedError());
      
      foreach (var fInfo in context.FunctionsCalled)
      {
        if (!fInfo.IsDefined)
          context.Errors.Add(new SemanticError("Function '{0}' isn't defined".Fmt(fInfo.GetSignature()), Line, Column));
      }
    }

    public List<DeclarationAST> Declarations { get; private set; }
    
  }
}
