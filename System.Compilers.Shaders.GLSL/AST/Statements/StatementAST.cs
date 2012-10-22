using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Statements
{
  public abstract class StatementAST : BaseAST, IVarSetter
  {
    protected StatementAST()
    {
    }

    public StatementAST(int line, int column)
      : base(line, column)
    {
    }

    public abstract List<VariableInfo> GetVarsSetted();

    public bool AlwaysReturn { get; set; }

    public bool NewScope { get; set; }   
  }
}
