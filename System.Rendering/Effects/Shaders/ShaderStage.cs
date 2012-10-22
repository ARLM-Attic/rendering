using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Rendering.Effects.Shaders
{
    public enum ShaderStage : int
    {
        Vertex = 0,
        Hull = 1,
        Domain = 2,
        Geometry = 3,
        Pixel = 4,
        Length = 5
    }
}