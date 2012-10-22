using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Utils;
using GLSLCompiler.Types;

namespace GLSLCompiler.AST
{
  public class NamedTypeSpecifier : BaseAST, ITypeSpecifier
  {
    public NamedTypeSpecifier()
    {
    }

    public NamedTypeSpecifier(int line, int column)
      : base(line, column)
    {
    }

    public NamedTypeSpecifier(string name, int line, int column)
      : base(line, column)
    {
      Name = name;
    }

    public override void CheckSemantic(SemanticContext context)
    {
      if (!context.Scope.DeepContainsType(Name))
        context.Errors.Add(new TypeNotDeclaredError(Name, Line, Column));
      else
        Type = context.Scope.GetTypeInfo(Name).Type;
    }

    public string Name { get; set; }

    public GLSLType Type { get; set; }
  }
}
