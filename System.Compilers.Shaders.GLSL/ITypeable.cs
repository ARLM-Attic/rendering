using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Types;

namespace GLSLCompiler
{
  public interface ITypeable
  {
    GLSLType Type { get; set; }
  }
}
