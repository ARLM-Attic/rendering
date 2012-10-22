using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST.Declarations
{
  public abstract class DeclarationAST : BaseAST
  {
    protected DeclarationAST()
    {
    }

    protected DeclarationAST(int line, int column)
      : base(line, column)
    {
    }

    protected DeclarationAST(ITypeSpecifier typeSpecifier, int line, int column)
      : base(line, column)
    {
      TypeSpecifier = typeSpecifier;
    }

    public ITypeSpecifier TypeSpecifier { get; set; }
  }
}
