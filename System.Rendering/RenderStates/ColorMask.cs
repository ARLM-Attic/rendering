using System;
using System.Collections.Generic;
using System.Text;

namespace System.Rendering.RenderStates
{
    /// <summary>
    /// Mask for RGBA component colors.
    /// </summary>
    public enum ColorMask
    {
        None = 0,
        Red = 1,
        Green = 2,
        Blue = 4,
        Alpha = 8,
        RGBA = 15
    }
}
