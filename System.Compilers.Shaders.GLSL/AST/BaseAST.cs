using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST
{
  public abstract class BaseAST : ICheckSemantic, ILocation
  {
    protected BaseAST()
    {
    }

    protected BaseAST(int line, int column)
    {
      Line = line;
      Column = column;
    }

    public abstract void CheckSemantic(SemanticContext context);

    public int Line { get; set; }

    public int Column { get; set; }
  }
}
