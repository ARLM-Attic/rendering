using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Statements
{
  public abstract class ControlStatementAST : StatementAST
  {
    protected ControlStatementAST()
    {
    }

    protected ControlStatementAST(int line, int column)
      : base(line, column)
    {
    }
  }
}
