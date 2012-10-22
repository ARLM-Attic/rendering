using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Rendering.RenderStates
{
    public struct AlphaTestState
    {
        public Compare Compare;

        public float Reference;

        public static readonly AlphaTestState Default = new AlphaTestState { Compare = Compare.Always, Reference = 0 };
    }
}
