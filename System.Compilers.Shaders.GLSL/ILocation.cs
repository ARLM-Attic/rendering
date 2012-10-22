using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler
{
  public interface ILocation
  {
    int Line { get; }

    int Column { get; }
  }
}
