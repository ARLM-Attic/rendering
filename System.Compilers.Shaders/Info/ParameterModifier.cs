using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Compilers.Shaders.Info
{
    [Flags]
    public enum ParameterModifier
    {
        None = 0,
        Const = 1,
        In = 2,
        Out = 4,
        /// <summary>
        /// Not used
        /// </summary>
        ByRef = 8,
        ConstIn = Const | In,
        ConstOut = Const | Out,
        InOut = In | Out,
        ConstInOut = Const | In | Out
    }
}
