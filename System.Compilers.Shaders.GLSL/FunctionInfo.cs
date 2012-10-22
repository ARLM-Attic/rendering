using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GLSLCompiler.Utils;
using GLSLCompiler.Types;

namespace GLSLCompiler
{
  public class FunctionInfo
  {
    public string Name { get; set; }

    public GLSLType ReturnType { get; set; }

    public List<ParamInfo> ParamInfos { get; set; }

    public List<bool> ParamsDefault { get; set; }

    public bool IsDefined { get; set; }

    public bool IsContructor { get; set; }

    public bool IsBuiltIn { get; set; } 
  }
}
