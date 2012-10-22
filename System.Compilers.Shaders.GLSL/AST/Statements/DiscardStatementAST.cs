using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.AST.Statements
{
  public class DiscardStatementAST : ControlStatementAST
  {
    public DiscardStatementAST()
    {
    }

    public DiscardStatementAST(int line, int column)
      : base(line, column)
    {
    }

    public override void CheckSemantic(SemanticContext context)
    {
      if (context.ShaderType != ShaderType.FramentShader)
        context.Errors.Add(new SemanticError("The discard statement is only allowed in a fragment shader", Line, Column));
      AlwaysReturn = false;
    }

    public override List<VariableInfo> GetVarsSetted()
    {
      return Enumerable.Empty<VariableInfo>().ToList();
    }
  }
}
