﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Compilers.Shaders.Info
{
    public interface ShaderMethodBase : ShaderMember
    {
        IEnumerable<ShaderParameter> Parameters { get; }
    }
}
