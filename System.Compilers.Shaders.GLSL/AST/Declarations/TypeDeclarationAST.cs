using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.AST.Declarations
{
  public abstract class TypeDeclarationAST : DeclarationAST
  {
    public TypeDeclarationAST()
    {
    }

    public TypeDeclarationAST(int line, int column)
      : base(line, column)
    {
    }

    protected TypeDeclarationAST(string name, int line, int column)
      : base(line, column)
    {
      Name = name;
    }

    public string Name { get; set; }

  }
}
