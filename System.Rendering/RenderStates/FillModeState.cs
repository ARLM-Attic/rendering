using System;
using System.Collections.Generic;
using System.Text;

namespace System.Rendering.RenderStates
{
    /// <summary>
    /// Defines supported filling operation for drawing builtins.
    /// </summary>
    public enum FillModeState
    {
        /// <summary>
        /// Full surface is drawn
        /// </summary>
        Fill = 3,
        /// <summary>
        /// Only edges are drawn
        /// </summary>
        Lines = 2,
        /// <summary>
        /// Only vertexes are drawn
        /// </summary>
        Points = 1,
        /// <summary>
        /// Full surface is drawn
        /// </summary>
        Default = Fill
    }  
}
