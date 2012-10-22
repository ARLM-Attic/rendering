using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.AST.Declarations;
using GLSLCompiler.Types;
using GLSLCompiler.Utils;

namespace GLSLCompiler.AST.Statements
{
  public class DeclarationStatementAST : StatementAST
  {
    public DeclarationStatementAST()
    {
    }

    public DeclarationStatementAST(int line, int column)
      : base(line, column)
    {
    }

    public DeclarationStatementAST(DeclarationAST declaration, int line, int column)
      : base(line, column)
    {
      Declaration = declaration;
    }

    public override void CheckSemantic(SemanticContext context)
    {
      if (Declaration.Is<FunctionDeclarationAST>() || Declaration.Is<FunctionDefinitionAST>())
        context.Errors.Add(new SemanticError("Function declarations/definitions are not allowed in this context.", Declaration.Line, Declaration.Column));
      else
      {
        Declaration.CheckSemantic(context);
        AlwaysReturn = false;
      }
    }

    public override List<VariableInfo> GetVarsSetted()
    {
      return Declaration.Is<IVarSetter>() ? 
              Declaration.Cast<IVarSetter>().GetVarsSetted() : 
              Enumerable.Empty<VariableInfo>().ToList();
    }

    public DeclarationAST Declaration { get; set; }
  }
}
