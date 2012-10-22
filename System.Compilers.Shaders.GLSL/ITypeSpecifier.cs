using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;
using GLSLCompiler.AST.Declarations;

namespace GLSLCompiler
{
  public interface ITypeSpecifier : ICheckSemantic, ILocation, ITypeable
  {
    string Name { get; }
  }
}
