using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLSLCompiler.Types
{
  public enum TypeQualifier
  {
    Default,
    Const,
    Attribute,
    Uniform,
    Varying,
    Centroid_Varying,
    Invariant,
    Invariant_Varying,
    Invariant_Centroid_Varying
  }

  public enum ParamQualifier
  {
    Default,
    Const,
    Const_In,
    In,
    Out,
    InOut,
  }
}
