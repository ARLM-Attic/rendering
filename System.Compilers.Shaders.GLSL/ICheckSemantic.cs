using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler
{
  public interface ICheckSemantic
  {
    void CheckSemantic(SemanticContext context);
  }
}
